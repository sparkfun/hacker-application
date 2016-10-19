class DroppedVerse
  include Mongoid::Document
  include Mongoid::Timestamps
  include ActionView::Helpers::DateHelper
  extend ActionView::Helpers::TextHelper

  belongs_to :user
  belongs_to :verse

  delegate :title, :text, :to => :verse

  field :location, type: Array
  index location: "2d"

  validates :location, presence: true

  ARCDEGREE_MILES = 69.0
  DEFAULT_RADIUS_MILES = 5
  FEET_IN_MILE = 5280

  def time_ago
    "Dropped #{distance_of_time_in_words(Time.zone.now, created_at)} ago"
  end

  def self.nearby lat, lon
    results = DroppedVerse.mongo_session.command({
      geoNear: "dropped_verses",
      near: [lat.to_f, lon.to_f],
      maxDistance: DEFAULT_RADIUS_MILES / ARCDEGREE_MILES,
      num: 100
    })['results']

    results.collect do |verse|
      dropped_verse = DroppedVerse.find(verse['obj']['_id'])
      {
        :id => verse['obj']['_id'],
        :title => dropped_verse.title,
        :text => dropped_verse.text,
        :time_ago => dropped_verse.time_ago,
        :location => dropped_verse.location,
        :distance => distance_in_words(verse['dis'])
      }
    end
  end

  private

  def self.distance_in_words distance
    miles = (distance * ARCDEGREE_MILES)
    if miles < 0.1
      format_feet(miles)
    else
      format_miles(miles)
    end
  end

  def self.format_miles distance_in_miles
    miles_rounded = distance_in_miles.round(1)
    miles_rounded = miles_rounded.to_i if (miles_rounded % 1).zero?
    pluralize(miles_rounded, 'mile')
  end

  def self.format_feet distance_in_miles
    feet_rounded = (distance_in_miles * FEET_IN_MILE).round
    "#{feet_rounded} feet"
  end
end

