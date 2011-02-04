class Part < ActiveRecord::Base
  has_many :assignments
  has_many :work_orders, :through => :assignments
  belongs_to :extended_description
  validates_uniqueness_of :ItemCode
  
  def self.find_all_with_extended_descriptions(options = {})
    with_scope :find => options do
      find :all, :joins => "LEFT OUTER JOIN `extended_descriptions` ON `parts`.extended_description_id = `extended_descriptions`.id"
    end
  end
end
