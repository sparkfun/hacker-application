module WorkOrderHelper
  def add_part_link(name)
    link_to_function name, "addPartRow()" #do |page|
        # page.insert_html :bottom, :parts_table_body, :partial => 'assignment_fields', :object => Assignment.new
    #end
  end
  
  def full_wo_status(code)
    case code.to_s
      when 'C' 
        "Closed" 
      when 'R' 
        "Open" 
      when 'F', 'E' 
        "Pending" 
      else
      end 
  end
end
