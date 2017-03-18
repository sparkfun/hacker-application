require File.expand_path('../../../spec_helper', __FILE__)

describe TestsController do
  render_views

  context 'when the "way" querystring param' do
    context 'is present' do
      it 'adds the file path to the partial' do
        get :index, '_way' => true
        response.body.should include "app/views/tests/_partial.erb"
      end
    end

    context 'is not present' do
      it 'does not add the file path to the partial' do
        get :index
        response.body.should_not include "app/views/tests/_partial.erb"
      end
    end
  end

  it 'does not show the path for the view' do
    get :index
    response.body.should_not include "index.erb"
  end

  it 'renders the view correctly' do
    get :index
    response.body.should include "I'm a view!"
  end

  it 'renders the partial correctly' do
    get :index
    response.body.should include "I'm a partial"
  end
end
