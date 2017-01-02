import React from 'react';
import ReactDOM from 'react-dom';
import SubPostComponent from './SubPostComponent.jsx';
import InputComponent from './InputComponent.jsx';
import TacoStr from './util/TacoStr.js';
import { Provider } from 'react-redux'


/**
 * This is an AddMany component's class.
 * This is the top level component for all the children components.
 * The hiearchy of parent to children components looks like this:
 * AddManyComponent > SubPostComponent > FieldsSubPostComponent > InputComponent.
 */
export default class AddManyComponent extends React.Component {

  /**
   * Perform some actions after the component mounts.
   * @return void
   */
  componentDidMount() {
    const { store } = this.context;
    this.loadSaved();
    store.dispatch({
      type: 'INIT',
      currentVariation: this.getDefaultVariation(),
      fieldName: this.props.fieldName,
      removedSubpostIds: [],
      searchButtonText: 'Show all',
    });
    this.addPostSaveHook();
  }


  /**
   * When the admin user clicks save,
   * do call some additional methods that check certain things.
   * @return bool;
   */
  addPostSaveHook() {
    let $ = jQuery;
    let self = this;
    $('#post').on('submit', function(e){
      if(!self.limitCheckMin()) {
        return false;
      }
      return true;
    });
  }


  /**
   * Get the field variation – a dropdown that gives the user
   * different field groups
   * @return mixed
   */
  getDefaultVariation() {
    let field_variations = this.props.fieldDefinitions[this.props.fieldName].field_variations;
    return (!this.props.isAddBySearch)
      ? (Object.keys(field_variations)[0])
      : 'default_variation'
  }


  /**
   * Render the component and its children
   * @return React component
   */
  render() {
    const { store } = this.context;
    const {
      subposts,
      removedSubpostIds,
      searchButtonText,
      loadingClass,
      resultsMessage,
      currentVariation
    } = store.getState();

    let variations = this.getFieldsVariationOptions();
    let removed = (removedSubpostIds === null || typeof removedSubpostIds === 'undefined' )
      ? []
      : removedSubpostIds;

    let renderedSubposts = [];
    if(typeof subposts != 'undefined') {
      subposts.forEach((s) => {
       renderedSubposts.push(
         <SubPostComponent
           key={s.postId}
           postId={s.postId}
           fieldsConfig={s.fieldsConfig}
           uniqid={s.postId}
           isAddBySearch={s.isAddBySearch}
           postReferenceInfo={s.postReferenceInfo}
           parentComponent={this} />
         );
     });
    }

    if(!this.props.isAddBySearch) {
      return (
        <div ref="main_container" className="addmany-component">

          <input
            name={ 'addmany_deleted_ids[' + this.props.fieldName + ']' }
            type="hidden"
            value={removed} />

          {
            (variations !== null)
              ? <select
                  value={currentVariation}
                  onChange={(e) => {
                    store.dispatch({
                      type: 'UPDATE_VARIATION',
                      variation: e.target.value
                    })
                  }}>
                  {this.getFieldsVariationOptionsHtml()}
                </select>
              : null
          }

          <button
            className="button"
            onClick={this.createNewSubPost.bind(this)}>Add new</button>

          <ul className="addmany-actual-values">{renderedSubposts}</ul>

        </div>
      );
    } else {
      return (
        <div ref="main_container" className="addmany-component with-addbysearch">

          <input
            name={ 'addmany_deleted_ids[' + this.props.fieldName + ']' }
            type="hidden"
            value={removed} />

          <input
            type="text"
            ref="searchableText"
            placeholder="search for posts"
            className={'addmany-searchable-field ' + loadingClass }
            onKeyPress={this.handleKeywordChange.bind(this)}
            onChange={this.handleKeywordChange.bind(this)} />

          <button
            className="button"
            onClick={this.searchPosts.bind(this)}>
            {searchButtonText}
          </button>

          <br />
          <br />

          <b>Search Results</b><em style={{float: 'right'}}>{resultsMessage}</em>
          <ul className="addmany-search-results">{this.renderSearchResults()}</ul>

          <br />
          <br />

          <b>Your Selection</b>
          <ul className="addmany-sorting-buttons">
            <li><button className="button" onClick={this.sortPostsAlpha.bind(this)}>Sort by Alphanumeric</button></li>

            {(this.props.isAddBySearch)
              ? <li><button className="button" onClick={this.sortPostsDate.bind(this)}>Sort by Post Date</button></li>
              : null }

            <li><button style={{background: '#FFF'}} className="button" onClick={this.sortPostsReverse.bind(this)}>Flip</button></li>
            <li><button style={{background: '#FFF'}} className="button" onClick={this.toggleCollapse.bind(this)}>Collapse/Expand</button></li>

          </ul>
          <ul className="addmany-actual-values">{renderedSubposts}</ul>

        </div>
      );
    }
  }


  /**
   * Collapse the AddMany subposts to a shortened height
   * so it's easier manipulate.
   * @param object e (event)
   * @return void
   */
  toggleCollapse(e) {
    e.preventDefault();
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let isGloballyMinimized = false;

    if(state.isGloballyMinimized) {
      subposts.forEach((s) => {
        s.isMinimized = false;
      });
      isGloballyMinimized = false;
    } else {
      subposts.forEach((s) => {
        s.isMinimized = true;
      });
      isGloballyMinimized = true;

    }
    store.dispatch({
      type: 'SET_MINIMIZED',
      subposts: subposts,
      isGloballyMinimized: isGloballyMinimized
    });
  }


  /**
   * Is the subpost collapsed?
   * @param Integer postId
   * @return bool
   */
  isMinimized(postId) {
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let isMinimized = false;

    subposts.forEach((s) => {
      if(postId === s.postId) {
        if(s.isMinimized === true) {
          isMinimized =  true;
        }
      }
    });
    return isMinimized;
  }


  /**
   * Get the search results
   * @return React component or null
   */
  renderSearchResults() {
    const { store } = this.context;
    const state = store.getState();
    if(typeof state.searchResultPosts == 'undefined') {
      return null;
    }
    let rendered = [];
    state.searchResultPosts.forEach((p) => {
      rendered.push(
        <li
          data-post-id={p.postId}
          key={p.postId}
          className="addbysearch-result postbox">
            <a
              href={'/wp-admin/post.php?post=' + p.postId + '&action=edit'}
              target="_blank">
              <span>{p.postTitle}</span>
              <span  className="dashicons dashicons-external"></span>
            </a>

            <button
              title="Click to add this post to the selection below."
              className="button"
              style={{float: 'right'}}
              onClick={this.cloneResultIntoSelection.bind({
                context: this,
                postReferenceInfo: {
                  postId: p.postId,
                  postTitle: p.postTitle
                }
              })}>
              <span className="dashicons dashicons-plus-alt"></span>
            </button>
        </li>
      );
    });
    return rendered;
  }


  /**
   * This is the handler for when a user modifies
   * text in the input when searching
   * @param object e (event)
   * @return void
   */
  handleKeywordChange(e) {
    const { store } = this.context;
    store.dispatch({
      type: 'SET_KEYWORDS',
      keywords: e.target.value,
      searchButtonText: (e.target.value) ? 'Search' : 'Show all'
    });
    if(e.which === 13) { /* Enter */
      e.preventDefault();
      this.searchPosts(e);
    }
  }


  /**
   * Set search results from keywords in the store's state
   * @param object e (event)
   * @return void
   */
  searchPosts(e) {
    e.preventDefault();
    var $ = jQuery;
    var self = this;
    const { store } = this.context;
    const state = store.getState();
    store.dispatch({
      type: 'SET_LOADING_STATE',
      loadingClass: 'is-loading'
    });
    $.ajax({
      url: AJAXSubmit.ajaxurl,
      method: 'post',
      data: {
        is_addbysearch: true,
        class_method: this.props.classMethod,
        field_assigned_to: this.props.fieldName,
        parent_id: this.props.parentPostId,
        keywords: state.keywords,
        action: 'AJAXSubmit',
        AJAXSubmit_nonce : AJAXSubmit.AJAXSubmit_nonce
      }
    }).success(function(d) {
      if(d.success) {
        self.addSearchResults(d.posts);
        return;
      }
      self.clearResults();
      store.dispatch({
        type: 'SET_LOADING_STATE',
        loadingClass: '',
        resultsMessage: 'The search returned zero results.'
      });
    });
  }


  /**
   * Set search results to an empty an array in the store
   * @return void
   */
  clearResults() {
    const { store } = this.context;
    const state = store.getState();
    store.dispatch({
      type: 'UPDATE_SEARCH_RESULTS',
      searchResultPosts: []
    });
  }


  /**
   * Add results from the search to the store
   * @param array posts
   * @return void
   */
  addSearchResults(posts) {
    const { store } = this.context;
    const state = store.getState();
    let searchResultPosts = [];

    posts.forEach((p) => {
      searchResultPosts.push(
        {
          postId: p.postId,
          postTitle: p.postTitle
        }
      );
    });

    store.dispatch({
      type: 'UPDATE_SEARCH_RESULTS',
      searchResultPosts: searchResultPosts,
      loadingClass: '',
      resultsMessage: searchResultPosts.length + ' results'
    });
  }


  /**
   * Add a result to the selection
   * @param object e (event)
   * @return void
   */
  cloneResultIntoSelection(e) {
    e.preventDefault();
    let context = this.context;
    context.createNewSubPost(e, this.postReferenceInfo);
  }


  /**
   * Get the html component for field variations
   * @return React component
   */
  getFieldsVariationOptionsHtml() {
    let variationOptions = this.getFieldsVariationOptions();
    let htmlVariationOptions = [];

    Object.keys(variationOptions).forEach((key) => {
      htmlVariationOptions.push(
        <option key={key} value={key}>{variationOptions[key]}</option>
      );
    });
    return htmlVariationOptions;
  }


  /**
   * Add a subpost row – happens after selecting from search results
   * or adding a new subpost from the create button
   * @param int postId
   * @param array fieldsConfig
   * @param array allData
   * @return void
   */
  addRow(postId, fieldsConfig = null, allData = null) {
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let postReferenceInfo = {};

    if(allData != null) {
      if(typeof allData.postReferenceInfo !== 'undefined') {
        postReferenceInfo = allData.postReferenceInfo;
      }
    }

    let subpost = {
      postId: postId,
      fieldsConfig: fieldsConfig,
      isAddBySearch: this.props.isAddBySearch,
      postReferenceInfo: postReferenceInfo
    }

    subposts.push(subpost);

    store.dispatch({
      type: 'ADD_SUBPOST',
      subposts: subposts
    });
  }


  /**
   * Add multiple rows at once from previously saved values
   * @param array loadedSubposts
   * @return void
   */
  addRows(loadedSubposts) {
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let self = this;
    loadedSubposts.forEach(function(s) {
      self.addRow(s.postId, s.fields, s);
    });
  }


  /**
   * If a component will mount, set some initial data in the store
   * @return void
   */
  componentWillMount() {
    const { store } = this.context;
    let subfields = this.props.fieldDefinitions[this.props.fieldName];
    store.dispatch({
      type: 'UPDATE_VARIATION',
      variation: Object.keys(subfields)[0]
    });
  }


  /**
   * Get options for the field variations dropdown
   * @return array
   */
  getFieldsVariationOptions() {
    let subfields = this.props.fieldDefinitions[this.props.fieldName].field_variations;
    if(Object.keys(subfields).length === 1) {
      return null;
    }
    let options = {};
    Object.keys(subfields).forEach(function(k) {
      options[k] = TacoStr.human(k);
    });
    return options;
  }


  /**
   * Force update the order of subposts in the user's selection
   * @return void
   */
  forceUpdateOrder() {
    let self = this;
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts;
    let $ = jQuery;
    let newArrayOfSubposts = [];

    $('tr.' + this.props.fieldName + ' ul').find('li').each(function(i) {
      let $this = $(this);
      subposts.forEach(function(s) {
        if(s.postId === $this.data('subpostId')) {
          newArrayOfSubposts.push(Object.assign({}, s, { order: i }));
        }
      });
    });
    store.dispatch({
      type: 'UPDATE_ORDERING',
      subposts: newArrayOfSubposts
    });
  }


  /**
   * Alert the user if the number of subposts exceeds the
   * max value of the AddMany field's settings
   * @return bool
   */
  limitCheckMax() {
    let self = this;
    const { store } = this.context;
    const state = store.getState();
    const subposts = state.subposts;
    if(!this.props.limitRange.length) {
      return false;
    }
    if(subposts.length === this.props.limitRange[1]) {
      alert('Item not added. You have reached the max number of items.')
      return true;
    }
  }


  /**
   * Alert the user if the number of subposts is below the
   * min value of the AddMany field's settings
   * @return bool
   */
  limitCheckMin() {
    let self = this;
    const { store } = this.context;
    const state = store.getState();
    const subposts = state.subposts;

    if(!this.props.limitRange.length) {
      return true;
    }
    if(subposts.length < this.props.limitRange[0]) {
      alert('You must have at least ' + this.props.limitRange[0] + ' items selected for the "' + TacoStr.human(this.props.fieldName) + '" field.' );
      return false;
    }
    return true;
  }


  /**
   * Check if a subpost is already in the user's selection
   * @param object postReferenceInfo
   * @return bool
   */
  subpostAlreadyInSelection(postReferenceInfo) {
    if(postReferenceInfo === null || typeof postReferenceInfo.postId === 'undefined') {
      return false;
    }
    const { store } = this.context;
    const state = store.getState();
    const subposts = state.subposts;

    let $bool = false;

    subposts.forEach((s) => {
      if(postReferenceInfo.postId === s.postReferenceInfo.postId) {
        $bool = true;
      }
    });

    return $bool;
  }

  /**
   * Create a new subpost by making a request to the server
   * If successful, the server will return info about the subpost.
   * @param object e (event)
   * @param object postReferenceInfo
   * @return void
   */
  createNewSubPost(e, postReferenceInfo=null) {
    e.preventDefault();
    if(this.limitCheckMax()) {
      return;
    }
    if(this.subpostAlreadyInSelection(postReferenceInfo)) {
      alert('This item is already in your selection.');
      return;
    }
    let $ = jQuery;
    let self = this;
    const { store } = this.context;

    $.ajax({
      url: this.props.submitURL,
      method: 'post',
      data: {
        parent_id: this.props.parentPostId,
        action: 'AJAXSubmit',
        AJAXSubmit_nonce: AJAXSubmit.AJAXSubmit_nonce,
        field_assigned_to: this.props.fieldName,
        current_variation: store.getState().currentVariation,
        post_reference_id: (postReferenceInfo != null)
          ? postReferenceInfo.postId
          : null
      }
    }).success(function(d) {
      if(d.success) {
        self.addRow(d.posts[0].postId, d.posts[0].fields, d)
      }
    });
  }


  /**
   * Get the order of a subpost in the user's selection
   * @param int postId
   * @return int
   */
  getOrder(postId) {
    const { store } = this.context;
    let subposts = store.getState().subposts;
    let self = this;
    let order = 0;
    subposts.forEach(function(subpost, index){
      if(postId === subpost.postId) {
        order = index;
        return;
      }
    });
    return order;
  }


  /**
   * Reverse the order of subposts in the user's selection
   * @param object e (event)
   * @return void
   */
  sortPostsReverse(e) {
    e.preventDefault();
    const { store } = this.context;
    let subposts = store.getState().subposts.slice(0).reverse();
    store.dispatch({
      type: 'UPDATE_ORDERING',
      subposts: subposts
    });
  }


  /**
   * Sort subposts alphabetically from the user's selection
   * @param object e (event)
   * @return void
   */
  sortPostsAlpha(e) {
    e.preventDefault();
    const { store } = this.context;
    let subposts = store.getState().subposts.slice(0).reverse();
    let subpostTitles = subposts.map((s) => {
      return s.postReferenceInfo.postTitle.toLowerCase();
    });

    subpostTitles.sort();
    let sorted = [];
    subpostTitles.forEach((title) => {
      subposts.forEach((s) => {
        if(title === s.postReferenceInfo.postTitle.toLowerCase()) {
          sorted.push(s);
        }
      })
    })

    store.dispatch({
      type: 'UPDATE_ORDERING',
      subposts: sorted
    });
  }


  /**
   * Sort subposts in the user's selection
   * by the order saved in the database from each subpost
   * @param array subposts
   * @return array
   */
  sortRowsByOrder(subposts) {
    subposts.sort(function(a, b) {
      return (a.order - b.order);
    });
    return subposts;
  }


  /**
   * Sort subposts by the date
   * @param object e (event)
   * @return void
   */
  sortPostsDate(e) {
    e.preventDefault();
    const { store } = this.context;
    let subposts = store.getState().subposts.slice(0);

    let subpostDates = subposts.map((s) => {
      return s.postReferenceInfo.postDate;
    });
    subpostDates.sort();
    let sorted = [];


    subpostDates.forEach((postDate) => {
      subposts.forEach((s) => {
        if(postDate === s.postReferenceInfo.postDate
          && sorted.indexOf(s) === -1) {
          sorted.push(s);
        }
      })
    });

    store.dispatch({
      type: 'UPDATE_ORDERING',
      subposts: sorted
    });
  }

  loadSaved(callback) {
    var $ = jQuery;
    var self = this;
    $.ajax({
      url: AJAXSubmit.ajaxurl,
      method: 'post',
      data: {
        get_by: true,
        field_assigned_to: this.props.fieldName,
        parent_id: this.props.parentPostId,
        action: 'AJAXSubmit',
        AJAXSubmit_nonce : AJAXSubmit.AJAXSubmit_nonce
      }
    }).success(function(d) {
      if(d.success) {
        let subposts = self.sortRowsByOrder(d.posts);
        self.addRows(subposts);
      }
    });
  }
}
AddManyComponent.contextTypes = { store: React.PropTypes.object };
AddManyComponent.defaultProps = {};
