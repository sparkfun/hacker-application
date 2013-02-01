/*-------------------------------------------
Security Settings
--------------------------------------------*/

   Notice = "DHTML Slide Tree Menu, Copyright (c) - 2001, OpenCube Inc. - www.opencube.com"
   code0 = 538
   code1 = 697
   sequence = "oc94127"
			limit_multiple_users = true

/*-------------------------------------------
Required menu Settings
--------------------------------------------*/

   menu_width = 150
   menu_height = 300

   scrolldelay = 5
   scrolljump = 2

   scrolldelay_nt = 5
   scrolljump_nt = 1;

   scrolldelay_ns4 = 50;
   scrolljump_ns4 = 6;

   urltarget = "_self"
   start_open_index = "1"
   display_urls_in_statusbar = true

   onload_statement = "none"
   codebase = "includes/"



/*-------------------------------------------
Required menu indenting and margins
--------------------------------------------*/

   main_itemheight = "auto"
   main_extend_height = 55

   sub_itemheight = "auto"
   sub1_indentwidth = 20
   sub2_indentwidth = 15

   main_marginleft = 5
   main_margin_topbottom = 8
   sub_marginleft = 0
   sub_margin_topbottom = 2



/*-------------------------------------------
Required font Settings
--------------------------------------------*/

   main_textcolor = "#333333"
   main_fontfamily = "Arial"
   main_fontsize = 12
   main_textdecoration = "normal"
   main_fontweight = "bold"
   main_fontstyle = "normal"
   main_hl_textcolor = "#000000"
   main_hl_textdecoration = "underline"

   sub_textcolor = "#333333"
   sub_fontfamily = "Arial"
   sub_fontsize = 11
   sub_textdecoration = "normal"
   sub_fontweight = "normal"
   sub_fontstyle = "normal"
   sub_hl_textcolor = "#000000"
   sub_hl_textdecoration = "underline"


/*---------------------------------------------
Required and optional background Settings
-----------------------------------------------*/


   /*---Required Settings---------*/ 

      background_color = "#eeeeee"
      
      main_item_bgcolor = "#cccccc"
      main_hl_item_bgcolor = "transparent"
  
      sub_item_bgcolor = "transparent"
      sub_hl_item_bgcolor = "transparent"

 
   /*---Optional Settings---------*/

      //main_background_color = "#000000"
      sub_background_color = "#eeeeee"

      main_background_image = "graphics/grad_back.gif"
      sub_level1_background_image = "graphics/grad_back.gif"
      sub_level2_background_image = "graphics/grad_back.gif"

      main_background_image = "graphics/grad_back.gif"
      ie5fix_sub_level1_background_image = "graphics/grad_back.gif"
      ie5fix_sub_level1_bg_image_height = 300
      ie5fix_sub_level2_background_image = "graphics/grad_back.gif"
      ie5fix_sub_level2_bg_image_height = 300   
	

      main_clipbg_after_items = true



/*-------------------------------------------
Required border settings -- set 'main_use_border'
and 'sub_use_border' to false for no borders
--------------------------------------------*/


   /*---All browsers Border settings-------------------*/
   
      main_use_border = true
      main_border_color = "#fffff0"
      main_hl_border_color = "#000000"

      sub_use_border = false;
      sub_border_color = "#ffffff"
      sub_hl_border_color = "#ff0000"


   /*---border offset distance (netscape 4.x bug workaround) 
        adjust untill right border is visible at edge----*/
      
      main_ns4fix_rightborder_offset = -2
      sub_ns4fix_rightborder_offset = -1


   /*---IE 5-up Only additional border settings---------*/
  
      main_use_2pixel_topbottom_border = false
      main_leftright_border_width = 1
      sub_leftright_border_width = 1



/*---------------------------------------------
Optional Icon Images
-----------------------------------------------*/

   icon_image0 = "graphics/samp2_bullet.gif"
   icon_image_wh0 = "13,8"
   icon_rollover0 = "graphics/samp2_bullet_hl.gif"
   icon_rollover_wh0 = "13,8"

   icon_image1 = "graphics/samp2_downarrow.gif"
   icon_image_wh1 = "13,10"
   icon_rollover1 = "graphics/samp2_downarrow_hl.gif"
   icon_rollover_wh1 = "13,10"

   icon_image2 = "graphics/samp2_downarrow.gif"
   icon_image_wh2 = "13,10"
   icon_rollover2 = "graphics/samp2_downarrow_main_hl.gif"
   icon_rollover_wh2 = "13,10"

   icon_image3 = "graphics/samp2_downarrow.gif"
   icon_image_wh3 = "13,10"
   icon_rollover3 = "graphics/samp2_downarrow.gif"
   icon_rollover_wh3 = "13,10"	
	

/*----------------------------------------------
Applet Overide Settings and additional applet
specific parameters - prefix any available parameter
name with 'applet_' to overide the setting when
running in applet mode
------------------------------------------------*/


   /*------for design purposes only---------*/

      //set the following param to true for 
      //testing the script in applet form.    
      ie5up_run_as_applet = false;
    

   /*------overriden script parameters---------*/
  
      applet_main_itemheight = 25
      applet_main_background_image = "graphics/samp2_app_mfix_mbg.gif"
      applet_main_use_border = false

   /*------Additional Applet Specific Parameters--*/

      applet_topoffset = -1;      

      applet_main_icon_indent = 7
      //applet_sub_icon_indent = 0

      applet_load_message_color = "#000000"
      applet_load_message = "Loading Images..."

      applet_main_font = "Helvetica, bold, 14" 
      applet_sub_font = "Helvetica, plain, 12"

   

/********************************************
THE FOLLOWING SECTIONS DEFINE THE ACTUAL
STRUCTURE AS WELL AS TEXT, URLS, AND ICONS 
THAT MAKE THE TREE MENU. - FOR HELP WITH 
INDEXING YOUR ITEMS REFER TO THE DOCUMENTATION.
*********************************************/

/*-------------------------------------------
Main Menu Item Settings
--------------------------------------------*/

main_text0 = "Add Listing"
icon_index0 = 2

main_text1 = "Browse Listings"
icon_index1 = 2

main_text2 = "Search Listings"
icon_index2 = 2

main_text3 = "Database Stats"
icon_index3 = 2
url3 = "database_statistics.aspx"

main_text4 = "Home"
icon_index4 = 3
url4 = "default.aspx"


/*---------------------------------------------
Sub Menu Item Settings
-----------------------------------------------*/

subdesc0_0 = "Business (no RE)"
subdesc0_1 = "Business with RE"
subdesc0_2 = "Commercial"
subdesc0_3 = "Farm &amp; Ranch"
subdesc0_4 = "Income Producing"
subdesc0_5 = "Residential"
subdesc0_6 = "Vacant Land"
icon_index0_0 = 0
icon_index0_1 = 0
icon_index0_2 = 0
icon_index0_3 = 0
icon_index0_4 = 0
icon_index0_5 = 0
icon_index0_6 = 0
url0_0 = "add_business_no_re.aspx"
url0_1 = "add_business_with_re.aspx"
url0_2 = "add_commercial_re.aspx"
url0_3 = "add_farm_ranch.aspx"
url0_4 = "add_income_producing_prop.aspx"
url0_5 = "add_residential.aspx"
url0_6 = "add_vacant_land.aspx"

subdesc1_0 = "Business (no RE)"
subdesc1_1 = "Business with RE"
subdesc1_2 = "Commercial"
subdesc1_3 = "Farm &amp; Ranch"
subdesc1_4 = "Income Producing"
subdesc1_5 = "Residential"
subdesc1_6 = "Vacant Land"
icon_index1_0 = 0
icon_index1_1 = 0
icon_index1_2 = 0
icon_index1_3 = 0
icon_index1_4 = 0
icon_index1_5 = 0
icon_index1_6 = 0
url1_0 = "browse_business_no_re.aspx"
url1_1 = "browse_business_with_re.aspx"
url1_2 = "browse_commercial_re.aspx"
url1_3 = "browse_farm_ranch.aspx"
url1_4 = "browse_income_producing_prop.aspx"
url1_5 = "browse_residential.aspx"
url1_6 = "browse_vacant_land.aspx"

subdesc2_0 = "Business (no RE)"
subdesc2_1 = "Business with RE"
subdesc2_2 = "Commercial"
subdesc2_3 = "Farm &amp; Ranch"
subdesc2_4 = "Income Producing"
subdesc2_5 = "Residential"
subdesc2_6 = "Vacant Land"
icon_index2_0 = 1
icon_index2_1 = 1
icon_index2_2 = 1
icon_index2_3 = 1
icon_index2_4 = 1
icon_index2_5 = 1
icon_index2_6 = 1

subdesc2_0_0 = "By MLS"
icon_index2_0_0 = 0
url2_0_0 = "search_business_no_re_by_mls.aspx"

subdesc2_1_0 = "By MLS"
icon_index2_1_0 = 0
url2_1_0 = "search_business_with_re_by_mls.aspx"

subdesc2_2_0 = "By MLS"
icon_index2_2_0 = 0
url2_2_0 = "search_commercial_re_by_mls.aspx"

subdesc2_3_0 = "By MLS"
icon_index2_3_0 = 0
url2_3_0 = "search_farm_ranch_by_mls.aspx"

subdesc2_4_0 = "By MLS"
icon_index2_4_0 = 0
url2_4_0 = "search_income_producing_prop_by_mls.aspx"

subdesc2_5_0 = "By MLS"
icon_index2_5_0 = 0
url2_5_0 = "search_residential_by_mls.aspx"

subdesc2_6_0 = "By MLS"
icon_index2_6_0 = 0
url2_6_0 = "search_vacant_land_by_mls.aspx"
