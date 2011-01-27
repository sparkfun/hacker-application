require 'time'
require 'date'

# Begin CONSTANTS 
CONSTANTS = {
  "now" => Time.new                                                   # DO NOT CHANGE: seed to find yesterday's date in gmt
}                                                                     
CONSTANTS.merge!({                                                    
  "end_date" => Date.parse(CONSTANTS["now"].getgm.to_s)-1,            # DO NOT CHANGE: Yesterday's date
########======================== Edit this section =================###############
  "num_files" => 7                                                    # Number of previous files (days) to load / process in each DataSet
})
CONSTANTS.merge!({
  "start_date" => CONSTANTS["end_date"] - (CONSTANTS["num_files"]-1), # DO NOT CHANGE: -1 for indexing offset
  "lines_in_file" => 86400,                                           # DO NOT CHANGE: number of secs in a day
  "offset" => 2000,                                                   # Change this to change the separation distance of the meter plots
  "gphone_user" => "mgl_admin",                                       # Global username for gPhone http file access
  "gphone_pass" => "gravity",                                         # Global password for gPhone https file access
  "www_ftp_server" => "ftp.microglacoste.com",                        # WWW Ftp Sever address if you are uploading to a website 
  "www_ftp_user" => "microgla",                                       # Ftp username
  "www_ftp_pass" => "microg422",                                      # Ftp Password
  "www_ftp_path" => "public_html",                                    # Path to navigate to on ftp server
  "tsf_file_path" => "tsf_files/",                                    # Path to where tsf_files should be stored
  "ftp_script_path" => "outputs/ftp.txt",                             # Path to where ftp script will be saved
  "plot_file_path" => "outputs/gPhoneComparisonPlot.png",             # Path/filename of where you want the plot saved (file will be overwritten)
  "data_file_path" => "outputs/plot_data.csv",                        # Path/filename of configuration file to be run into gnuplot (file will be overwritten)
  "gnuplot_script_path" => "outputs/gnuplot_script.conf",             # Path/filename of the script to run to gnuplot
  "earthquake_file_path" => "inputs/earthquakes.csv"                  # Path/filename of the file containing earthquake events that should be plotted
})
# End CONSTANTS

# Each meter in this section will have it's data downloaded and plotted
# Format: [meter_name, server_ip, location]
# NOTE: meter_name MUST be _identical_ to the name that appears in the files on the file system
# (i.e. if you enter gPhone 95 when the file is gPhone 095 this script will error and exit)
meters = [
  ["gPhone 095","10.0.1.119","Boulder, CO"],
  ["gPhone 097","216.254.148.51","Toronto, Canada"]
]
##########=================  End Editable Section =================#######################

# Add function "mean" to all Arrays
class Array
  # return floating point cumulative sum of all elements
  def sum
    inject(0.0) { |result, el| result + el } 
  end
  
  # return mean of the array.
  def mean
    sum / size
  end
end


# TsfFile is a particularly formatted file used in gravity and seismology
# it can be imported into anothe program called Tsoft.  Here I've created
# methods so that the data I want can be extracted, checked, and reformatted.
class TsfFile
  attr_reader :name
  
  def initialize(name)
    @f = File.open(name,"r")
    @name = name
  end
  
  #loop through file and extract time and gravity
  #return array with columns time and gravity
  def get_time_and_corrected_gravity_data
    output = []
    @f.rewind
    until @f.eof 
      cols = parse_row(@f.gets)
      output << cols unless cols === false
    end
    output
  end
  
  # pull out columns
  def parse_row(data_str)
    grav_col = 6
    tide_col = 8
    date_cols = 0..2
    time_cols = 3..5
    return false unless data_str.match /^\d/  # ignore header rows, all other rows start with a number
    cols =  data_str.split
    cor_grav = cols[grav_col].to_f - cols[tide_col].to_f
    output = [cols[date_cols].join('-') + "-" + cols[time_cols].join(":"), cor_grav]
    return output
  end
end

# DataSet contains all pieces of information about the meter whose files it will contain
# Also, the data_array instance variable will contain the combined output of all of the TsfFiles
# that the DataSet contains (after process_files is called).
# The Dataset contains methods to download necessary data files, delete unused files, 
# and process (i.e. parse through) the files.
class DataSet
  attr_accessor :meterName, :server, :files, :location, :offset
  attr_reader :data_array, :mean
  
  def initialize(meterName, server, location)
    defined?(@@data_set_count).nil? ? @@data_set_count = 0 : @@data_set_count += 1
    @meterName = meterName
    @server = server
    @location = location
    @files = []
    @filenames = []
    @data_array = []
    @offset = @@data_set_count * CONSTANTS["offset"]
    
    # create the array of files that composes the DataSet.  Fileame is determined by a convention.  Number of days
    # to download is determined by CONSTANTS["num_files"]
    (0..CONSTANTS["num_files"]-1).each do |i|
      filename = "#{(CONSTANTS["start_date"]+i).year}_#{"%03d" % (CONSTANTS["start_date"]+i).yday.to_i}_#{@meterName}.tsf"
      # if File.file?(CONSTANTS["tsf_file_path"] + filename)
      #   puts "#{filename} exists: skipping download."
      # else
        download_data_file(filename)
      # end
      @files << TsfFile.new(CONSTANTS["tsf_file_path"] + filename)
      @filenames << CONSTANTS["tsf_file_path"] + filename
    end
  end
  
  # Download data using wget from each meter's http server.
  def download_data_file(filename)
    #download the .tsf file from the gphone computers using wget
    puts "Downloading: \"#{@server}/gmonitor_data/#{filename}\""
    `wget -c --directory-prefix=#{CONSTANTS["tsf_file_path"]} --user=#{CONSTANTS["gphone_user"]} --password=#{CONSTANTS["gphone_pass"]} \"#{@server}/gmonitor_data/#{filename}\"`
  end
  
  # if there is a TSF file in the os Tree that is not in the files array remove it
  # as it is not needed anymore
  def delete_irrelevant_data_files
    shell_file_names = Dir.glob("#{CONSTANTS["tsf_file_path"]}*#{meterName}.tsf")
    shell_file_names.each do |shell_file_name|
      unless @filenames.include? shell_file_name
        puts "Deleting: #{shell_file_name}"
        File.delete(shell_file_name)
      end
    end
  end
  
  # extract the desired data (will be datetime and corrected gravity) from each file
  # and concatenate it onto the data_arrray variable
  def process_files
    @files.each do |file|
      puts "Processing #{file.name}.."
      puts "  Extracting Corrected Gravity Data..."
      file_data = file.get_time_and_corrected_gravity_data
      file_data.each do |row|
        @data_array << row
      end
    end
    find_mean
  end
  
  def num_gaps
    CONSTANTS["lines_in_file"] * CONSTANTS["num_files"] - @data_array.size
  end
  
  def fix_gaps(length_diff)
    puts "  DataSet is too short: adding #{length_diff} empty lines"
    nil_array = []
    (0..length_diff-1).each do
      nil_array << [nil,nil]
    end
    @data_array += nil_array
  end
  
  def normalize
    @data_array.map! {|x| [x[0], x[1] -= @mean]}
  end
  
  def find_mean
    @mean = @data_array.transpose[1].mean
  end
  
  def add_offset
    @data_array.map! {|x| [x[0], x[1] += @offset]}
  end
  
  def convert_to_mGals
    @data_array.map! {|x| [x[0], x[1] / 1000]}
  end
end

# this object creates a new file that contains the script info for the plotting program.
# earthquakes are plotted with a vertical line (arrow) and the coordinates are found in 
# the @quake_file.  Data_sets are necessary to find where the meters are located and what
# their names are.
class GnuplotScript
  def initialize(file_path,data_sets)
    @file = file_path
    @quake_file = File.new(CONSTANTS["earthquake_file_path"],'r')
    @y_max = data_sets.last.offset / 1000 + 1.5   # divide by 1000 to convert offset to mGal (should clean this up) and
    
    @meter_names = []  # will contain each meter name from each data_set
    @locations = []    # the locations from each data set
    data_sets.each do |data_set|
      @meter_names << data_set.meterName
      @locations << data_set.location
    end
  end

  # used by create method
  def quake_str
    quakes = ""
    @quake_file.each do |line|
      cols = line.split(",")
      if Date.parse(cols[0]) <= CONSTANTS['end_date'] && Date.parse(cols[0]) >= CONSTANTS['start_date']
        quakes << %Q/set arrow from '#{cols[0]}', graph 0 to '#{cols[0]}', graph 1 nohead lw 3\n/
        quakes << %Q/set label right "#{cols[1].chomp}\\n#{cols[0]}" at '#{cols[0]}', graph 0.98\n/
      end
    end
    quakes
  end
  
  # used by create method
  def loc_str
    locs = nil
    @locations.each do |location|
      locs.nil? ? locs = location : locs << " and #{location}"
    end
    locs
  end
  
  # used by create method
  def using_str
    using = nil
    @meter_names.each_index do |n|
      if using.nil?
        using = "'#{CONSTANTS["data_file_path"]}' using #{n*2+1}:#{n*2+2} index 0 title '#{@meter_names[n]}(#{@locations[n]})\' with lines"
      else    
        using << ", '#{CONSTANTS["data_file_path"]}' using #{n*2+1}:#{n*2+2} index 0 title '#{@meter_names[n]}(#{@locations[n]})\' with lines"
      end
    end
    using
  end

  # write all info to file
  def create
    f = File.new(@file, 'w')
    f.print %Q/set terminal png size 1600,900
set output '#{CONSTANTS["plot_file_path"]}'

# Graph settings
set xdata time
set timefmt '%Y-%m-%d-%H:%M:%S'
set output '#{CONSTANTS["plot_file_path"]}'
set xrange ['#{CONSTANTS["start_date"]}-00:00:00':'#{CONSTANTS["end_date"]}-23:59:59']
set yrange [-1.5:#{@y_max}]
set grid

# Labels
set xlabel "Date\\nTime"
set ylabel 'Acceleration (mGals)'
set title 'Ground Motion recorded between #{loc_str}'
set key bmargin center horizontal box\n

# Earthquakes
#{quake_str}

# Plot settings
set datafile separator ','
plot #{using_str}

# Save Image
screendump/
    f.close
  end

  # run file to gnuplot
  # NOTE: gnuplot must be in path
  def execute
    `gnuplot #{CONSTANTS['gnuplot_script_path']}`
  end
end

# Object that can generate and run an ftp script for uploading
class FtpScript
  def initialize(filename, user, pass, uploads)
    @filename = filename   # file path/name on filesystem
    @user = user           # ftp username
    @pass = pass           # ftp password
    @uploads = uploads     # Array of files to send
  end
  
  # Create file
  def create
    f = File.new(@filename,'w')
    
    f.puts @user
    f.puts @pass
    f.puts "cd public_html"
    f.puts "binary"
    @uploads.each do |upload|
      f.puts "put #{upload}"
    end
    f.puts "bye"
    f.close
  end
  
  # Run script to ftp program.
  # NOTE: ftp must be in path
  def execute
    `ftp -s:#{filename} #{CONSTANTS['www_ftp_server']}`
  end
end

data_sets = []             # Array containing the DataSet objects for each meter entry
master_set = []            # Array that will contain the compilation of all DataSets' data_arrays

meters.each do |meter|
  data_sets << DataSet.new(meter[0],meter[1],meter[2])
end

data_sets.each do |data_set|
  data_set.delete_irrelevant_data_files
  data_set.process_files
  data_set.normalize
  data_set.add_offset
  data_set.convert_to_mGals
  data_set.fix_gaps(data_set.num_gaps) if data_set.num_gaps > 0
  master_set += data_set.data_array.transpose
end

puts "Writing datafile (#{CONSTANTS["data_file_path"]})..."
fout = File.new("#{CONSTANTS["data_file_path"]}",'w')
master_set.transpose.each do |line|
  fout.puts line.join ","
end
fout.close

puts "Creating gnuplot script..."
gnuplot_script = GnuplotScript.new(CONSTANTS['gnuplot_script_path'], data_sets)
gnuplot_script.create
puts "Running script to gnuplot..."
gnuplot_script.execute

puts "Creating ftp script..."
ftp_script = FtpScript.new(CONSTANTS['ftp_script_path'], CONSTANTS['www_ftp_user'], CONSTANTS['www_ftp_pass'], [CONSTANTS['plot_file_path']])
ftp_script.create

puts "Uploading image via ftp..."
ftp_script.execute
