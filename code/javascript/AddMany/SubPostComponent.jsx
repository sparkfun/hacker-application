import React from 'react';
import FieldsSubPostComponent from './FieldsSubPostComponent.jsx';


/**
 * This is a SubPost React component's class.
 * A SubPost is a single item in a list that is definied by the user.
 * A SubPost consists of many fields which are also React components.
 */
export default class SubPostComponent extends React.Component {

  /**
   * Perform some actions after the component mounts.
   * @return void
   */
  componentDidMount(){
    jQuery('tr.' + this.props.parentComponent.props.fieldName)
      .find('.addmany-actual-values')
      .sortable(this.getSortableConfig());
  }


  /**
   * The render method for this component
   * @return React component
   */
  render() {
    const { store } = this.context;
    let order = this.props.parentComponent.getOrder(this.props.postId);
    let postReferenceId = null;
    const state = store.getState();

    let classReordering = (state.repositionedSubpostId === this.props.postId)
      ? 'addmany-currently-reordering '
      : '';

    let classMinimized = (this.props.parentComponent.isMinimized(this.props.postId))
      ? 'addmany-currently-minimized '
      : '';

    if(this.props.postReferenceInfo != null) {
      if(typeof this.props.postReferenceInfo.postId != 'undefined')  {
        postReferenceId = this.props.postReferenceInfo.postId;
      }
    }

    return (
      <li
        className={'subpost-component addmany-result postbox ' + classReordering + classMinimized}
        order={order}
        data-subpost-id={this.props.postId}
        data-post-reference-id={postReferenceId}
        key={this.props.postId} >

        <table>
          <FieldsSubPostComponent
            postReferenceInfo={this.props.postReferenceInfo}
            fields={this.props.fieldsConfig}
            subpostId={this.props.postId}
            isAddBySearch={this.props.isAddBySearch}
            order={order} />
        </table>

        <button
          className="btn-addmany-delete button"
          onClick={this.removeRow.bind(this)}>
          <span className="dashicons dashicons-no"></span>
        </button>

        {this.getMinimizeButton()}
        {this.getOrderButtons()}

      </li>
    );
  }


  /**
   * Get a React component for the minimize button
   * @return React component
   */
  getMinimizeButton() {
    if(this.props.isAddBySearch && !Object.keys(this.props.fieldsConfig).length) {
      return null;
    }
    let dashIcon = (this.props.parentComponent.isMinimized(this.props.postId))
      ? 'editor-expand'
      : 'minus';
    return (
      <button
        className="btn-addmany-minimize button"
        onClick={this.minimize.bind(this)} >
        <span className={'dashicons dashicons-' + dashIcon}></span>
      </button>
    );
  }


  /**
   * Minimize/collapse a subpost to a shortened height
   * @param object e (event)
   */
  minimize(e) {
    e.preventDefault();
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let subposts_filtered = [];

    subposts.forEach((s) => {
      if(s.postId === this.props.postId) {
        s.isMinimized = (!this.props.parentComponent.isMinimized(s.postId));
      }
      subposts_filtered.push(s);
    });

    store.dispatch({
      type: 'SET_MINIMIZED',
      subposts: subposts_filtered,
    });
  }


  /**
   * Get a React component of the order buttons
   * The order buttons are an alternative to drag and dropping subposts
   * @return React component or null
   */
  getOrderButtons() {
    const {store} = this.context;
    const state = store.getState();
    if(state.subposts.length > 1 && !this.props.parentComponent.isMinimized(this.props.postId)) {
      return (
        <div>
          <button
            onClick={this.moveItemOrderUp.bind(this)}
            className="addmany-btn-order-up">
            <span className="dashicons dashicons-arrow-up-alt"></span>
          </button>

          <button
            onClick={this.moveItemOrderDown.bind(this)}
            className="addmany-btn-order-down">
            <span className="dashicons dashicons-arrow-down-alt"></span>
          </button>
        </div>
      )
    }
    return null;
  }


  /**
   * Move a subpost up and change the order
   * @param object e (event)
   * @return void
   */
  moveItemOrderUp(e) {
    e.preventDefault();
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let reOrdered = [];
    let parentComponent = this.props.parentComponent;
    let order = parentComponent.getOrder(this.props.postId);
    if(order === 0) {
      return;
    }

    let repositionedSubpostId = null;

    // offset order of all subposts
    subposts.forEach((s) => {
      let sOrder = parentComponent.getOrder(s.postId);
      if(sOrder === order) {
        repositionedSubpostId = this.props.postId;
        reOrdered.push(
          Object.assign({}, s, { order: sOrder - 1 })
        );
      } else {
        repositionedSubpostId = this.props.postId;
        reOrdered.push(
          Object.assign({}, s, { order: sOrder + 1 })
        );
      }
    });

    reOrdered.sort((a, b) =>{
      return a.order > b.order
    });

    store.dispatch({
      type: 'UPDATE_ORDERING',
      subposts: reOrdered,
      repositionedSubpostId: repositionedSubpostId
    });

    setTimeout(function(){
      store.dispatch({
        type: 'UI_ORDERING_DONE',
        repositionedSubpostId: null
      });
    }, 2000)
  }


  /**
   * Move a subpost down and change the order
   * @param object e (event)
   * @return void
   */
  moveItemOrderDown(e) {
    e.preventDefault();
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts.slice(0);
    let reOrdered = [];
    let parentComponent = this.props.parentComponent;
    let order = parentComponent.getOrder(this.props.postId);
    if(order === subposts.length -1) {
      return;
    }

    let repositionedSubpostId = null;

    // offset order of all subposts
    subposts.forEach((s) => {
      let sOrder = parentComponent.getOrder(s.postId);
      if(sOrder === order) {
        repositionedSubpostId = this.props.postId;
        reOrdered.push(
          Object.assign({}, s, { order: sOrder + 1 })
        );
      } else {
        repositionedSubpostId = this.props.postId;
        reOrdered.push(
          Object.assign({}, s, { order: sOrder - 1 })
        );
      }
    });

    reOrdered.sort((a, b) =>{
      return a.order > b.order
    });

    store.dispatch({
      type: 'UPDATE_ORDERING',
      subposts: reOrdered,
      repositionedSubpostId: repositionedSubpostId
    });

    setTimeout(function(){
      store.dispatch({
        type: 'UI_ORDERING_DONE',
        repositionedSubpostId: null
      });
    }, 2000)
  }


  /**
   * Remove a subpost from the user's selection
   * @param object e (event)
   * @return void
   */
  removeRow(e) {
    e.preventDefault();
    let self = this;
    const { store } = this.context;
    const state = store.getState();
    let subposts = state.subposts;
    let new_subposts_array = [];

    subposts.forEach(function(o){
      if(self.props.postId === o.postId) {
        return;
      }
      new_subposts_array.push(o);
    });
    // remove subpost
    store.dispatch({
      type: 'REMOVE_SUBPOST',
      subposts: new_subposts_array
    });
    // update order
    this.props.parentComponent.forceUpdateOrder();
    this.updateDeletedValues(self.props.postId);
  }


  /**
   * After a subpost is removed, push the subpost id to the deleted values.
   * Upon saving the parent post (save/update button),
   * these subposts will be removed.
   * @param int id
   * @return void
   */
  updateDeletedValues(id) {
    const { store } = this.context;
    const state = store.getState();
    let removedSubpostIds = state.removedSubpostIds.slice(0);
    removedSubpostIds.push(id)
    store.dispatch({
      type: 'UPDATE_REMOVED',
      subpostIds: removedSubpostIds
    })
  }


  /**
   * Get the sortable configuration for all subposts
   * The sorting library is already available in the WordPress admin.
   * @return object
   */
  getSortableConfig(){
    let self = this;
    let $ = jQuery;
    let fieldName = self.props.parentComponent.props.fieldName;
    let $domActualValues = $('tr.' + fieldName + ' .addmany-actual-values');

    return {
      revert: 100,
      start: function(e, ui) {

        ui.item.addClass('addmany-currently-reordering');
        $domActualValues.find('li').each(function(i) {
          $(this).find('.wysiwyg').each(function () {
            $(this).hide();
            tinyMCE.execCommand('mceRemoveEditor', false, $(this).attr('id'));
          });
        });
      },
      stop: function(e, ui) {
        const { store } = self.context;
        let subposts = store.getState().subposts.slice(0)

        ui.item.removeClass('addmany-currently-reordering');
        let newArrayOfSubposts = [];
        $domActualValues.find('li').each(function(i) {
          let $this = $(this);
          subposts.forEach(function(s) {
            if(s.postId === $this.data('subpostId')) {
              newArrayOfSubposts.push(Object.assign({}, s, { order: i }));
            }
          });
          $(this).find('.wysiwyg').each(function () {
            $(this).show();
            tinyMCE.execCommand('mceAddEditor', true, $(this).attr('id'));
          });
        });
        store.dispatch({
          type: 'UPDATE_ORDERING',
          subposts: newArrayOfSubposts
        });
      }
    };
  }
}

SubPostComponent.contextTypes = { store: React.PropTypes.object };
SubPostComponent.defaultProps = { order: null };
