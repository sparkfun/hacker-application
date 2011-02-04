class WorkOrder < ActiveRecord::Base
  has_many :assignments
  has_many :parts, :through => :assignments
  has_and_belongs_to_many :users
  
  validates_associated :assignments
  
  after_update :save_assignments
  
  accepts_nested_attributes_for :assignments, :allow_destroy => :true,
   :reject_if => proc {|attrs| :all_blank}
   
  def self.all_initialized
    wos = self.all
    rval = true
    wos.each do |wo|
      rval = false if !wo.initialized
    end
    return rval
  end
  # used when assignments are resubmitted after a validation error
  def new_assignment_attributes=(assignment_attributes)
    assignment_attributes.each do |attributes|
      assignments.build(attributes)
      if attributes.include?("audit") 
        PocMailer.deliver_audit_notification(attributes[:part_id],attributes[:user_id])
      end
    end
  end
  
  # for changes to existing (used primarily when a submission error occurs)
  def existing_assignment_attributes=(assignment_attributes)
    assignment_attributes.each do |attributes|
      assignment = assignments.detect { |a| a.id == attributes[:id].to_i }
      assignment.attributes = attributes
      if assignment_attributes.include?('audit') 
        PocMailer.deliver_audit_notification(assignment_attributes[:part],assignment_attributes[:user_id])
      end
    end
  end
  
  def save_assignments
    assignments.each do |assignment|
      if assignment.should_destroy?
        assignment.destroy
      else
        assignment.save(false)
      end
    end
  end
end
