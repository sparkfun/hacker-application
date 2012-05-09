class WorkOrdersController < ApplicationController
  filter_resource_access
  helper_method :sort_column, :sort_direction
  
  def index
    @work_orders = WorkOrder.find(:all, :order => "#{sort_column} #{sort_direction}")

    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :xml => @work_orders }
    end
    
  end
  
  # New work orders can only be created by the MAS200 data import script
  # def new
  #   @work_order = WorkOrder.build()
  #   @pocs = Role.find_by_name("Poc")
  #   @pocs = @pocs.users || []
  # end

  def show
    #wont let me include users anymore...why?
    @work_order = WorkOrder.find(params[:id], :include => [:assignments, :parts])
    @wo_pocs = @work_order.users
    
    
    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :xml => @work_orders }
    end
  end

  def edit
    #wont let me include users anymore...why?
    @work_order = WorkOrder.find(params[:id], :include => [{:assignments => :user}, :parts]) 
    @pocs = Role.find_by_name("Poc", :include => :users)
    @pocs = @pocs.users
    @wo_pocs = @work_order.users
    @show_protected = current_user.roles.exists?("admin")
  end

  def update
    if current_user.roles.exists?(:name => "admin")
      params[:work_order][:user_ids] ||= []
    end
    @work_order = WorkOrder.find(params[:id])
#    @wo_pocs = @work_order.pocs
    @pocs = Role.find_by_name("Poc")
    @pocs = @pocs.users || []
    
    respond_to do |format|
      if @work_order.update_attributes(params[:work_order])
        format.html { redirect_to(@work_order, :notice => 'Work Order was successfully updated.') }
        format.xml  { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml  { render :xml => @work_order.errors, :status => :unprocessable_entity }
      end
    end
  end

  def destroy
    @work_order = WorkOrder.find(params[:id])
    @work_order.destroy
    
    respond_to do |format|
      format.html { redirect_to(workorders_url) }
      format.xml  { head :ok }
    end
  end
  
  private
  def sort_column
    WorkOrder.column_names.include?(params[:sort]) ? params[:sort] : 'id'
  end
  
  def sort_direction
    %w[asc desc].include?(params[:direction]) ? params[:direction] : 'asc'
  end
end
