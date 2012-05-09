# Methods added to this helper will be available to all templates in the application.
module ApplicationHelper
    def admin?
    !current_user.nil? && current_user.roles.exists?(:name => "admin")
  end
  
  def logged_in?
    !current_user.nil?
  end
  
  def ajax_sortable(column, title = nil)
    title ||= column.titleize
    direction = (column == sort_column && sort_direction == "asc") ? "desc" : "asc"
    link_to_function title, "changeSort('#{column}','#{direction}');retrieveData($('date_form'))"
  end
  
  def sortable(column, title = nil)
    title ||= column.titleize
    direction = (column == sort_column && sort_direction == "asc") ? "desc" : "asc"
    link_to title, :sort => column, :direction => direction
  end
  
  def sql_to_local(time_str = nil)
    return nil if time_str.nil? 
    utc_time = Time.parse(time_str.to_s)
    utc_time.localtime
    return utc_time.strftime('%b %d, %Y %I:%M:%S %p')
  end
end
