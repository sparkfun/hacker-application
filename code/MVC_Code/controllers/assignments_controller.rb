class AssignmentsController < ApplicationController
  layout "application", :except => [:show,:resolve]
  helper_method :sort_column, :sort_direction, :start_date, :end_date
  
  def index
    @assignments = Assignment.find(:all, :include => [:user, :work_order, :part], :order => "#{sort_column} #{sort_direction}, created_at DESC")

    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :xml => @assignments }
    end
  end

  def show
    if(start_date == '--' && end_date == '--')
      @assignments = Assignment.find(:all, :include => [:user, :work_order, :part], :order => "#{sort_column} #{sort_direction}, created_at DESC")
    else
      @assignments = Assignment.find_all_by_dates(start_date, end_date, :include => [:user, :work_order, :part], :order => "#{sort_column} #{sort_direction}, created_at DESC")
    end

    respond_to do |format|
      format.html # show.html.erb
      format.xml  { render :xml => @assignment }
    end
  end

  def new
    @assignment = Assignment.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml  { render :xml => @assignment }
    end
  end

  def edit
    @assignment = Assignment.find(params[:id])
  end

  def create
    @assignment = Assignment.new(params[:assignment])

    respond_to do |format|
      if @assignment.save
        format.html { redirect_to(@assignment, :notice => 'Assignment was successfully created.') }
        format.xml  { render :xml => @assignment, :status => :created, :location => @assignment }
      else
        format.html { render :action => "new" }
        format.xml  { render :xml => @assignment.errors, :status => :unprocessable_entity }
      end
    end
  end

  def update
    @assignment = Assignment.find(params[:id])

    respond_to do |format|
      if @assignment.update_attributes(params[:assignment])
        format.html { redirect_to(@assignment, :notice => 'Assignment was successfully updated.') }
        format.xml  { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml  { render :xml => @assignment.errors, :status => :unprocessable_entity }
      end
    end
  end
  
  def resolve
    @assignment = Assignment.find(params[:id])
    @assignment.update_attribute('audit_resolved',sql_datetime)
    PocMailer.deliver_audit_resolution_notification(@assignment.part_id, @assignment.user)
  end
  
  def destroy
    @assignment = Assignment.find(params[:id])
    @assignment.destroy

    respond_to do |format|
      format.html { redirect_to(assignments_url) }
      format.xml  { head :ok }
    end
  end
  
  private
  def sort_column
    Assignment.column_names.include?(params[:sort]) ? params[:sort] : 'created_at'
  end
  
  def sort_direction
    %w[asc desc].include?(params[:direction]) ? params[:direction] : 'asc'
  end
  
  def start_date
    params[:start_date] || '--'
  end
  
  def end_date
    params[:end_date] || '--'
  end

end