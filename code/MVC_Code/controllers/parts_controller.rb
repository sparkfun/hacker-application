class PartsController < ApplicationController
  layout "application", :except => [:show_ajax, :search]
  # GET /parts
  # GET /parts.xml
  def index
    @parts = Part.find_all_with_extended_descriptions(:limit => 1000)

    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :xml => @parts }
    end
  end

  def show_ajax
    @parts = Part.id_like(params[:number].gsub(/([\\\"\'])/, '\\\\\1'))
  end

  def search
    @parts = Part.id_like(params[:number]).find_all_with_extended_descriptions(:conditions => ["(`parts`.description LIKE ? OR body LIKE ?)","%#{params[:description].gsub(/([\\\"\'])/, '\\\\\1')}%","%#{params[:description].gsub(/([\\\"\'])/, '\\\\\1')}%"])
  end
end
