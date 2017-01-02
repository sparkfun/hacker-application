import React from 'react';
import ReactDOM from 'react-dom';
import AddManyComponent from './AddManyComponent.jsx';
import { createStore } from 'redux';
import expect from 'expect';
import { Provider } from 'react-redux'
import { connect } from 'react-redux'
require('../scss/addmany.scss');

/**
 * This file is responsible for intializing the AddMany components
 * from looping over the DOM and finding fields
 * that have the "[data-addmany=1]" attribute.
 * It also contains the store for all the data.
 */


/*
 * This component and children uses a store
 * for making data more manageable.
*/
const reducer = (state = {}, action) => {
  switch(action.type) {
    case 'INIT':
      return Object.assign({}, state, {
        fieldName: action.fieldName,
        subposts: [],
        currentVariation: action.currentVariation,
        removedSubpostIds: [],
        searchResultPosts: [],
        searchButtonText: 'Show all',
        isGloballyMinimized: false
      });
    case 'UPDATE_VARIATION':
      return Object.assign({}, state, {
        currentVariation: action.variation,
      });
    case 'ADD_SUBPOST':
      return Object.assign({}, state, {
        subposts: action.subposts
      });
    case 'REMOVE_SUBPOST':
      return Object.assign({}, state, {
        subposts: action.subposts,
      });
    case 'UPDATE_ORDERING':
      return Object.assign({}, state, {
        subposts: action.subposts,
        repositionedSubpostId: action.repositionedSubpostId
      });
    case 'UPDATE_REMOVED':
      return Object.assign({}, state, {
        removedSubpostIds: action.subpostIds,
      });
    case 'UPDATE_SEARCH_RESULTS':
      return Object.assign({}, state, {
        searchResultPosts: action.searchResultPosts,
        loadingClass: action.loadingClass,
        resultsMessage: action.resultsMessage
      });
    case 'SET_KEYWORDS':
      return Object.assign({}, state, {
        keywords: action.keywords,
        searchButtonText: action.searchButtonText
      });
    case 'SET_SEARCH_BUTTON_TEXT':
      return Object.assign({}, state, {
        searchButtonText: action.searchButtonText,
      });
    case 'SET_LOADING_STATE':
      return Object.assign({}, state, {
        loadingClass: action.loadingClass,
        resultsMessage: action.resultsMessage
      });
    case 'UI_ORDERING_DONE':
    return Object.assign({}, state, {
      hasReordered: action.hasReordered,
      repositionedSubpostId: action.repositionedSubpostId
    });
    case 'SET_MINIMIZED':
    return Object.assign({}, state, {
      subposts: action.subposts,
      isGloballyMinimized: action.isGloballyMinimized
    });
    default:
      return state;
  }
}

// Connect the store with the AddManyComponent
const AddManyComponentWithStore = connect(state => state)(AddManyComponent);

(function($){
  let $elements = $('[data-addmany=1]');
  if(!$elements.length) return;

  /**
   * We're only using jQuery here because it's already included with WordPress
   * and makes DOM selecting a little easier.
   */
  $elements.each(function(){
    // Loop over fields on page that have "[data-addmany=1]" and apply the AddManyComponent to each.
    let fieldDefinitions = window.field_definitions[$(this).attr('name')];

    // TODO: Refactor
    // There are way too many props here (Only my second React app).
    ReactDOM.render(
      <Provider store={createStore(reducer)}>
        <AddManyComponentWithStore
          dbValue={$(this).val()}
          submitURL={AJAXSubmit.ajaxurl}
          parentPostId={$('#post_ID').val()}
          domElement={$(this).parent()}
          fieldDefinitions={window.field_definitions}
          fieldName={$(this).attr('name')}
          limitRange={window.field_definitions[$(this).attr('name')]['limit_range']}
          isAddBySearch={typeof fieldDefinitions.is_addbysearch !== 'undefined'}
          classMethod={(typeof fieldDefinitions.class_method !== 'undefined' ) ? fieldDefinitions.class_method : null}
          />
      </Provider>,
      $(this).parent()[0]
    );
  });
})(jQuery);
