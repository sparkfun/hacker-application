class Assignment < ActiveRecord::Base
  belongs_to :part
  belongs_to :work_order
  belongs_to :user
 
  validates_numericality_of :user_id
  validates_numericality_of :qty_in, :greater_than => 0, :if => "qty_out.to_i==0", :only_integer => true
  validates_numericality_of :qty_out, :greater_than => 0, :if => "qty_in.to_i==0", :only_integer => true
  validates_numericality_of :qty_in, :qty_out, :only_integer => true
  
  
  attr_accessor :should_destroy
  
  def should_destroy?
    should_destroy.to_i == 1
  end
  
  def self.find_all_by_dates(start_date, end_date, options = {})
    with_scope :find => options do
      sd = Time.parse(start_date.to_s)
      ed = Time.parse(end_date.to_s)
      find :all, :conditions => ["? <= `created_at` AND `created_at` <= ?", sd.getutc, ed.getutc]
    end
  end
end
