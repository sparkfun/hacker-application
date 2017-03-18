<?php 
/*
 * FILE: class.admin.php
 * PURPOSE: admin object to show CMS buttons on web pages when logged in,
 *          and display CMS forms (editing content, slideshows, etc.)
 *          on actual front-end web pages -- part of jvajva.com
 *  AUTHOR: berto 8/11/2014        
 * 
*/

class admin {

	function __construct($page) {
		$this->page = ($page == 'home') ? 'index.php' : $page . ".php";
		$this->btndiv = '<div class="admin_btns">';
		$this->buttons = '<hr /><p class="buttonz">Hit "Submit" to save changes:</p><p class="buttonz smaller"><input name="submit_btn" type="submit" value="Submit" class="btn" title="Submits edits" />  &nbsp;&nbsp;|&nbsp;&nbsp; <input name="cancel_btn" id="cancel_btn" type="button" value="Cancel" class="btn" title="Cancels current edits" onclick="javascript:window.location.href=\''.$this->page.'?result=cancelled\';" /></p>';
		$this->submit_buttons = '<hr /><p class="buttonz">Hit "Submit" to save changes:</p><p class="buttonz smaller"><input name="submit_btn" type="submit" value="Submit" class="btn" title="Submits edits" /></p>';
		$this->close_buttons = '<p class="buttonz" style="margin-top: 10px;"><input name="close_btn" id="close_btn" type="button" value="X&nbsp; Close (Done Editing)" class="btn" title="Close" onclick="javascript:if (checkSlideForm(\''.$this->page.'\')) window.location.href=\''.$this->page.'\';" /></p>';
		$this->enddiv = "<div class=\"relax\">&nbsp;</div>\n</div>\n";
	}
	
	// FUNCTION FOR THE ADMIN BUTTONS
		
	function show_admin_buttons($type, $ypos=0, $xpos=0, $_id=0) {
		$result = "";	
		$result .= $this->btndiv;
		$result .= "\n";
		$result .= '<form action="'.$this->page.'" method="GET">';
		$result .= "\n";
		$result .= '<input name="edit" type="hidden" value="'.$type;
		$result .= ($_id > 0) ? ' ' . $_id : '';
		$result .= '" />';
		$result .= "\n";
		$result .= '<input name="submit" class="abs btn" type="submit"';
		$result .= (($ypos != 0) ||($xpos != 0)) ? ' style="margin: '.$ypos.'px 0 0 '.$xpos.'px;"' : '';
		$result .= ' value="';
                $result .= (strpos($type, "New") !== false) ? 'Enter '.$type : 'Edit '.$type;
                $result .= '" />';
		$result .= "\n</form>";
		$result .= $this->enddiv;
		
		return $result;
	}
	
	// FUNCTION FOR INITIALIZING EACH OF THE EDIT DIVS
	
	function init_div($divclass, $ypos=0, $xpos=0) {
		$result = '';
		$result .= '<div id="edit_div" class="'.$divclass.'"';
		$result .= (($ypos != 0) ||($xpos != 0)) ? ' style="margin: '.$ypos.'px 0 0 '.$xpos.'px;"' : '';
		$result .= '>';
		return $result;
	}
	
	// FUNCTION FOR CREATING ALL THE EDIT DIVS
	
	function construct_div($divclass, $innards, $ret, $standardbuttons='true', $ypos=0, $xpos=0) {
		$result = '';
		$result .= $this->init_div($divclass, $ypos, $xpos);
		$result .= "\n";
		$result .= '<form id="editForm" name="editForm" action="'.$this->page.'" enctype="multipart/form-data" method="POST"';
		if (($divclass == 'admin_edit_slides')||($divclass == 'edit_image_div')) {
			$result .= ' onsubmit="javascript:return valSlideForm(\''.$this->page.'\');"';
		}
                if ($divclass == 'admin_edit_office') {
			$result .= ' onsubmit="javascript:return valOfficeForm(\''.$this->page.'\');"';
		}
		if ($divclass == 'admin_edit_career') {
			$result .= ' onsubmit="javascript:return valCareerForm(\''.$this->page.'\');"';
		}
                if ($divclass == 'admin_edit_team') {
			$result .= ' onsubmit="javascript:return valTeamForm(\''.$this->page.'\');"';
		}
		$result .= '>';
		$result .= "\n";
		$result .= $innards;
		$result .= "\n";
		$result .= '<input name="'.$ret.'_page" id="'.$ret.'_page" type="hidden" value="'.$this->page.'" />';
		$result .= "\n";
		$result .= ($standardbuttons == 'true') ? $this->buttons : $this->submit_buttons;
		if ($divclass == 'admin_edit_slides') {
			$result .= $this->close_buttons;
		}
		$result .= "\n</form>";
		$result .= $this->enddiv;
			
		return $result;
	}

	// INDIVIDUAL EDIT DIVS FUNCTIONS
        
    function headline_edit_div($body_text) {
		$div_class = "headline_edit";
		$htmlstr = "";
		$htmlstr .= '<input name="head_edit" id="head_edit" class="headline_field" type="text" value="'.$body_text.'" size="30" maxlength="200" />';	
		return $this->construct_div($div_class, $htmlstr, 'headline');
	}

	function content_edit_div($body_text) {
		$div_class = "content_edit";
		$htmlstr = "";
		$htmlstr .= '<textarea name="page_content" id="page_content" class="tinymce"';
		$htmlstr .=  '>'.$body_text.'</textarea>';	
		return $this->construct_div($div_class, $htmlstr, 'content');
	}
        
        function content_edit_div_right($body_text) {
		$div_class = "content_edit_right";
		$htmlstr = "";
                $htmlstr .= '<textarea name="page_content_right" id="page_content_right" class="tinymce2"';
		$htmlstr .= '>'.htmlentities($body_text, ENT_QUOTES, "UTF-8").'</textarea>';
		
		return $this->construct_div($div_class, $htmlstr, 'content_right');
	}
	
	function slideshow_images_edit_div($extStr, $destination, $w, $h, $gallery_result, $maxw) {  // main slideshow edit div
		$htmlstr = "";
		$htmlstr .= '<p><b>Edit Slideshow Images:</b> <span class="smaller">(Shown here at 1/8 size - optimal size is '.$w.'px x '.$h.'px)</span></p>';
		$htmlstr .= "\n";
                $htmlstr .= ($this->page == 'index.php') ? '<p>Drag an image' : '<p>Click an image to edit its caption, drag it around';
		$htmlstr .= ' to change its order in the slideshow, or drag it to the box below to delete. <br /><span class="smaller">(No changes are saved until you hit Submit.)</span></p>';
		$htmlstr .= "\n";
		$htmlstr .= '<div>';
		$htmlstr .= "\n";
		$current_images = array();
		if ($gallery_result) {
			$htmlstr .= '<ul id="sortable">';
			$htmlstr .= "\n";
			foreach ($gallery_result as $slide_row) { 
				$slide_url = $slide_row['pic_url'];
				$current_images[] = $slide_url;
				$so = $slide_row['sort_order'];
				$id = $slide_row['id'];
				$htmlstr .= '<li><span class="num">&nbsp;'.$so.'</span>';
				if ($this->page != 'index.php') {
				      $htmlstr .= '<a href="'.$this->page.'?edit=thisSlide&id='.$id.'" onclick="javascript:return checkSlideForm(\''.$this->page.'\');" title="Click to edit, or drag to re-sort.">';
				}
				$htmlstr .= '<img src="includes/thimg.php?h=69&img='.$destination.'/'.$slide_url.'" border="0" alt="" />';
				if ($this->page != 'index.php') {
				      $htmlstr .= '</a>';
				}
				$htmlstr .= '<input name="newsort[]" type="hidden" value="'.$so.'" /></li>';
				$htmlstr .= "\n";
			}
			$htmlstr .= "</ul>\n";
			$htmlstr .= '<input name="changes" id="changes" type="hidden" value="" />';
			$htmlstr .= "\n";
			$htmlstr .= '<div class="relax">&nbsp;</div>';
			$htmlstr .= '<div id="picdrop"><br /><span class="small">(Drop a pic here<br />to delete)</span></div>';
                        $htmlstr .= "\n";
		} else {
			$htmlstr .= "<p>No images in slideshow yet.</p>";
		}	
		$htmlstr .= "\n";
		$htmlstr .= '</div>';
		$htmlstr .= "\n";
		$htmlstr .= '<table align="left" cellspacing="0" cellpadding="0" style="padding-top: 10px;">';
		$htmlstr .= "\n";
		$htmlstr .= '<tr valign="top"><td style="padding-right: 20px; border-right: 1px solid #999;">';
		$htmlstr .= '<p><b>You Can Upload a New Image Here</b><br /><span class="smaller">('.$extStr.' files only):</span></p>';
		$htmlstr .= "\n";
		$htmlstr .= '<p><input name="file" id="file" type="file" title="Lets you choose a picture to upload" /></p>';
		$htmlstr .= "\n";
        if ($this->page == 'index.php') {
                    $htmlstr .= '<input name="caption" id="caption" type="hidden" value="" onkeyup="javasript: this.value = this.value.toUpperCase();" />';
                    $htmlstr .= "\n";
                    $htmlstr .= '<input name="sub_cap" id="sub_cap" type="hidden" value="" />';
        } else {
                    $htmlstr .= '<p class="smaller">Picture caption (shows on thumbnails):<br />';
                    $htmlstr .= "\n";
                    $htmlstr .= '<textarea name="caption" id="caption" class="caption" rows="1" cols="20" onkeyup="javasript: this.value = this.value.toUpperCase();"></textarea></p>';
                    $htmlstr .= '<p class="smaller">Sub-caption: (If entered, puts a caption on main pic. <br />To start on a new line, enter a line return first.)<br />';
                    $htmlstr .= "\n";
                    $htmlstr .= '<textarea name="sub_cap" id="sub_cap" class="sub_cap" rows="2" cols="20"></textarea></p>';
        }
		$htmlstr .= '</td>';
		$htmlstr .= "\n";
		$p_arr = get_files($destination, $current_images, $maxw);
		if (!empty($p_arr)) {
			$htmlstr .= '<td style="padding-left: 20px;"><p>Or, check boxes to add other pictures:</p>';
			$htmlstr .= '<ul>';
			foreach ($p_arr as $key => $img) {
				$htmlstr .= '<li style="line-height: 18px; padding-bottom: 2px;">';
				$htmlstr .= '<input name="add_imgs[]" class="smaller" type="checkbox" value="'.$img.'" />';
				$htmlstr .= '<img src="includes/thimg.php?h=18&img='.$destination.'/'.$img.'" style="vertical-align: middle;" /> &nbsp;<span class="smaller">'.$img.'</span></li>';
			}
			$htmlstr .= '</ul>';
			$htmlstr .= '</td>';
		}
		$htmlstr .= '</tr></table>';

		$htmlstr .= "\n";
		$htmlstr .= '<div class="relax">&nbsp;</div>';
		
		return $this->construct_div('admin_edit_slides', $htmlstr, 'slides_image', true, -10, -10);

	}
	
	function slide_edit_div($id, $gallery_result) { // individual slide edit div
		$selurl = '';
		foreach ($gallery_result as $gallery) {
			if ($gallery['id'] == $id) {
				$selurl = $gallery['pic_url'];
				$selcap = br2nl($gallery['caption']);
                                $subcap = br2nl($gallery['sub_cap']);
				break;
			}
		}
		if (!empty($selurl)) {
			$htmlstr = '';
			$htmlstr .= '<p><b>Edit Image:</b> (shown here at approx. 1/4 size)</p>';
			$htmlstr .= "\n";
			$htmlstr .= "\n";
			$htmlstr .= '<img src="images2/'.$selurl.'" style="max-width: 171px; max-height: 95px;" />';
			$htmlstr .= "\n";
			$htmlstr .= '<p class="smaller">&nbsp;<br />Picture caption (shows on thumbnails):<br />';
			$htmlstr .= "\n";
			$htmlstr .= '<textarea name="caption" id="caption" class="caption" rows="1" cols="20" onkeyup="javasript: this.value = this.value.toUpperCase();">'.$selcap.'</textarea></p>';
			$htmlstr .= "\n";
                        $htmlstr .= '<p class="smaller">&nbsp;<br />Sub-caption: (If entered, puts a caption on main pic. <br />To start on a new line, enter a line return first.)<br />';
			$htmlstr .= "\n";
			$htmlstr .= '<textarea name="sub_cap" id="sub_cap" class="sub_cap" rows="2" cols="20">'.$subcap.'</textarea></p>';
			$htmlstr .= "\n";
			$htmlstr .= '<input name="selid" id="selid" type="hidden" value="'.$id.'" /><input name="selurl" id="selurl" type="hidden" value="'.$selurl.'" />';
			$htmlstr .= "\n";
			$htmlstr .= '<input name="file" id="file" type="hidden" value="old" />';
			$htmlstr .= "\n";
		} else {
			$htmlstr .= '<p><i>There was a problem. Please hit Cancel to continue.</i></p>';
		}
				
		$htmlstr .= '<div class="relax">&nbsp;</div>';
		
		return $this->construct_div('edit_image_div', $htmlstr, 'singleslide_image');
	
	}
	
	function office_edit_div($o_id, $extStr, $destination, $maxw, $maxh, $o_img=null, $o_title=null, $o_address=null, $o_maplink=null) {
		$htmlstr = '';	
		$htmlstr .= "\n";
		$htmlstr .= '<table border="0" cellspacing="0" cellpadding="0">';
		$htmlstr .= "\n";
		$htmlstr .= '<tr valign="top"><td>';
		$htmlstr .= '<div style="width:'.$maxw.'px;height:'.$maxh.'px;margin-bottom: 15px;">';
		if ($o_id == "new") {
			$htmlstr .= '<p><b>Enter New Office</b></p>'; 
			$htmlstr .= "\n";
		} else {
			$htmlstr .= '<img src="images2/' . $o_img . '" width="'.$maxw.'" height="'.$maxh.'" alt="" />';
		}
		$htmlstr .= '<input name="oid" id="oid" type="hidden" value="'.$o_id.'" />';
		$htmlstr .= '</div>';
		$htmlstr .= "\n";
		$htmlstr .= '<p><input name="ins_title" id="ins_title" class="aichfour" type="text" value="'.$o_title.'" /></p>';
		$htmlstr .= '<p><textarea name="ins_address" id="ins_address" class="o_address_field" rows="6" cols="30">'.br2nl($o_address).'</textarea></p>';
		$htmlstr .= '<p class="smaller">Link to Map (or leave blank): <br /><input name="ins_maplink" id="ins_maplink" class="o_maplink_field" type="text" value="'.$o_maplink.'" /></p>';
		$htmlstr .= "\n";
		$htmlstr .= '</td><td width="20">&nbsp;</td><td>';
		$htmlstr .= "<p><b>Upload an Image</b> <br /><span class=\"smaller\">($extStr files only - <br />final size to be $maxw x $maxh pixels):</span></p>";
		$htmlstr .= "\n";
		$htmlstr .= '<p><input name="file" id="file" type="file" title="Lets you choose a picture to upload" onchange="updatePic(this.form);" /></p>';
		$htmlstr .= "\n";
                $htmlstr .= '<p class="smaller">Chosen picture:<br /><input name="chosen_pic" id="chosen_pic" class="pic_field" type="text" value="'.$o_img.'" size="30" maxlength="200" onfocus="javascript:this.blur();" /></p>';
                $htmlstr .= "\n";
		$htmlstr .= '<p class="smaller" style="padding-bottom: 0;">Or, pick from existing pictures of the same size:</p>';
		$p_arr = get_files($destination, array(), $maxw);
		if (!empty($p_arr)) {
                    $htmlstr .= '<ul class="imgs" style="float: left; margin-right: 20px;">';
                    $i = 0;
                    foreach ($p_arr as $key => $img) {
                        $htmlstr .= '<li style="line-height: 18px; padding-bottom: 2px;">';
                        $htmlstr .= '<a href="javascript:setImg(\''.$key.'\', \''.$img.'\');" class="'.$key;
                        if ($img == $o_img) {
                           $htmlstr .= ' chosen';
                        }
                        $htmlstr .= '"><img src="includes/thimg.php?h=18&img='.$destination.'/'.$img.'" style="vertical-align: middle;" /></a>&nbsp;<a href="javascript:setImg(\''.$key.'\', \''.$img.'\');" class="\''.$key.'\'"> <span class="smaller">'.$img.'</span></a></li>';
                        $i++;
                        if ((count($p_arr) > 6)&&($i == ceil(count($p_arr) / 2))) {
                        $htmlstr .= '</ul><ul class="imgs" style="float: left;">';
                        }
                    }
                    $htmlstr .= '</ul>';
		} else {
                    $htmlstr .= '<p>(No images currently in folder.)</p>';
		}
		$htmlstr .= '</td></tr></table>';
		$htmlstr .= "\n";
		$htmlstr .= '<div class="relax">&nbsp;</div>';
		$htmlstr .= "\n";
		
		return $this->construct_div('admin_edit_office', $htmlstr, 'office', 'true', -15, -15);
	}
	
	function career_edit_div($c_id, $c_title=null, $c_datetime=null, $c_location=null, $c_description=null, $c_qualifications=null, $t_file=null) {
		$htmlstr = '';
		$htmlstr .= ($c_id == "new") ? '<p><b>Enter New Career</b></p>' : '<p><b>Edit Career</b></p>'; 
		$htmlstr .= "\n<hr />";
		$htmlstr .= "\n<p>";
		$htmlstr .= '<b>Location:</b> <input name="c_location" id="c_location" class="career_fld" type="text" value="'.$c_location.'" size="20" maxlength="250" />';
		$htmlstr .= "<br />\n";
		$htmlstr .= '<b>Career Description:</b>';
		$htmlstr .= "<br />\n";
		$htmlstr .= '<textarea name="c_description" id="c_description" class="career_fld" rows="6" cols="50">'.$c_description.'</textarea>';
		$htmlstr .= "<br />\n";
		if ($c_datetime == null) $c_datetime = date('Y-m-d H:i:s');
		$htmlstr .= '<b>Date &amp; Time Posted (determines sort order):</b> <input name="c_datetime" id="c_datetime" type="text" value="'.$c_datetime.'" size="20" maxlength="250" />';
		$htmlstr .= "<br />\n";
		$htmlstr .= '<b>Qualifications (insert a separate line for each):</b>';
		$htmlstr .= "<br />\n";
		$htmlstr .= '<textarea name="c_qualifications" id="c_qualifications" class="career_qual" rows="6" cols="50">'.$c_qualifications.'</textarea>';
		$htmlstr .= "<br />\n";
		$htmlstr .= '<b>Career Title (shows on application email):</b> <input name="c_title" id="c_title" type="text" value="'.$c_title.'" size="20" maxlength="50" />';
		$htmlstr .= "</p>\n";
		$htmlstr .= '<p><b>Upload a PDF (optional):</b>';
		$htmlstr .= "&nbsp;&nbsp;";
		$htmlstr .= '<input name="file" id="file" type="file" title="Lets you choose a PDF file to upload" onchange="updatePdf(this.form);" /></p>';
		$htmlstr .= "\n";
        $htmlstr .= '<p class="smaller">Chosen file:';
		$htmlstr .= "&nbsp;";
		$htmlstr .= '<input name="chosen_pdf" id="chosen_pdf" class="pic_field" type="text" value="'.$t_file.'" size="30" maxlength="200" onfocus="javascript:this.blur();" />';
		$htmlstr .= "&nbsp;&nbsp;&nbsp;";
		if ($t_file != null) {
			$htmlstr .= '<input name="delfile" id="delfile" class="smaller" type="checkbox" value="1" onclick="toggleDel(\''.$t_file.'\')" /> Check to delete current PDF';
		}
		$htmlstr .= '</p>';
		$htmlstr .= "\n";
		$htmlstr .= '<input name="cid" id="cid" type="hidden" value="'.$c_id.'" />';
		$htmlstr .= "\n";
		$htmlstr .= '<div class="relax">&nbsp;</div>';
		$htmlstr .= "\n";
		
		return $this->construct_div('admin_edit_career', $htmlstr, 'career', 'true', -15, -15);
	}
        
    function team_edit_div($t_id, $extStr, $destination, $maxw, $maxh, $t_img=null, $t_name=null, $t_title=null, $t_email=null, $t_location=null) {
		$htmlstr = '';	
                $htmlstr .= '<input name="tid" id="tid" type="hidden" value="'.$t_id.'" />';
                $htmlstr .= "\n";
		if ($t_id == "new") {
			$htmlstr .= '<p><b>New Entry</b></p>'; 
			$htmlstr .= "\n";
		} 
                $htmlstr .= '<p><input name="is_spacer" id="is_spacer" type="checkbox" value="1" onclick="ckTeamBox();"';
                $htmlstr .= ($t_name == "spacer") ? ' checked': '';
                $htmlstr .= ' /> Check if this is to be a marker</p>';
                $htmlstr .= '<div id="spacerImgDiv" style="display: ';
                $htmlstr .= ($t_name == "spacer") ? 'block': 'none';
                $htmlstr .= '"><p><img src="images/jva_mark_6.jpg" width="'.$maxw.'" height="'.$maxh.'" alt="" style="float: left; margin-right: 20px;" />';
                $htmlstr .= 'This will put a logo-marker in this grid position. <br />(Its color will be determined automatically.)</p>';
                $htmlstr .= '</div>';
                $htmlstr .= "\n";
                $htmlstr .= '<div id="showHide"';
                $htmlstr .= ($t_name == "spacer") ? ' style="display: none;"': '';
                $htmlstr .= '>';
		$htmlstr .= '<table border="0" cellspacing="0" cellpadding="0">';
		$htmlstr .= "\n";
		$htmlstr .= '<tr valign="top"><td>';
		$htmlstr .= '<div style="width:'.$maxw.'px;height:'.$maxh.'px;margin-bottom: 15px;">';
		if ($t_id != "new") {
                    $htmlstr .= '<img src="images2/' . $t_img . '" width="'.$maxw.'" height="'.$maxh.'" alt="" />';
                }
                $htmlstr .= '</div>';
                $htmlstr .= "\n";
                $htmlstr .= '<p>Name:<br/><textarea name="ins_name" id="ins_name" class="team_name_field" rows="2" cols="20" style="width: '.$maxw.'px;">'.br2nl($t_name).'</textarea></p>';
                $htmlstr .= "\n";
                $htmlstr .= '<p>Title:<br/><input name="ins_title" id="ins_title" class="team_title_field" type="text" style="width: '.$maxw.'px;" value="'.$t_title.'" /></p>';
                $htmlstr .= "\n";
                $htmlstr .= '<p>Email:<br/><input name="ins_email" id="ins_email" class="team_email_field" type="text" style="width: '.$maxw.'px;" value="'.$t_email.'" /></p>';
                $htmlstr .= "\n";
                $htmlstr .= '<p>City:<br/><input name="ins_location" id="ins_location" class="team_location_field" type="text" style="width: '.$maxw.'px;" value="'.$t_location.'" /></p>';
                $htmlstr .= "\n";
                $htmlstr .= '</td><td width="20">&nbsp;</td><td>';
                $htmlstr .= "<p><b>Upload an Image</b> <br /><span class=\"smaller\">($extStr files only - <br />final size to be $maxw x $maxh pixels):</span></p>";
                $htmlstr .= "\n";
                $htmlstr .= '<p><input name="file" id="file" type="file" title="Lets you choose a picture to upload" onchange="updatePic(this.form);" /></p>';
                $htmlstr .= "\n";
                $htmlstr .= '<p class="smaller">Chosen picture:<br /><input name="chosen_pic" id="chosen_pic" class="pic_field" type="text" value="'.$t_img.'" size="30" maxlength="200" onfocus="javascript:this.blur();" /></p>';
                $htmlstr .= "\n";
                $htmlstr .= '<p class="smaller" style="padding-bottom: 0;">Or, pick from existing pictures of the same size:</p>';
                $p_arr = get_files($destination, array(), $maxw);
                if (!empty($p_arr)) {
                    $htmlstr .= '<ul class="imgs" style="float: left; margin-right: 20px;">';
                    $i = 0;
                    foreach ($p_arr as $key => $img) {
                        $htmlstr .= '<li style="line-height: 18px; padding-bottom: 2px;">';
                        $htmlstr .= '<a href="javascript:setImg(\''.$key.'\', \''.$img.'\');" class="'.$key;
                        if ($img == $t_img) {
                           $htmlstr .= ' chosen';
                        }
                        $htmlstr .= '"><img src="includes/thimg.php?h=18&img='.$destination.'/'.$img.'" style="vertical-align: middle;" /></a>&nbsp;<a href="javascript:setImg(\''.$key.'\', \''.$img.'\');" class="\''.$key.'\'"> <span class="smaller">'.$img.'</span></a></li>';
                        $i++;
                        if ((count($p_arr) > 6)&&($i == ceil(count($p_arr) / 2))) {
                        $htmlstr .= '</ul><ul class="imgs" style="float: left;">';
                        }
                    }
                    $htmlstr .= '</ul>';
                } else {
                    $htmlstr .= '<p>(No images currently in folder.)</p>';
                }

		$htmlstr .= '</td></tr></table></div>';
		$htmlstr .= "\n";
		$htmlstr .= '<div class="relax">&nbsp;</div>';
		$htmlstr .= "\n";
		
		return $this->construct_div('admin_edit_team', $htmlstr, 'team', 'true', -38, -100);
	}

}

?>