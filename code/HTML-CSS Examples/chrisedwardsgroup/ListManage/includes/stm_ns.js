if((s45)&&(scrolldelay<11))scrolldelay=11;if(!s45){scrolldelay=scrolldelay_ns4;scrolljump=scrolljump_ns4;}else scrolljump=scrolljump_nt;captureEvents(Event.RESIZE);captureEvents(Event.MOUSEMOVE);captureEvents(Event.MOUSEDOWN); window.onmousemove=s89;window.onmousedown=s68;window.onresize=s77;if(main_use_border){main_marginleft -=3;main_margin_topbottom -=3;}if(sub_use_border){sub_marginleft -=3;sub_margin_topbottom -=3;}sw="<STYLE TYPE='text/javascript'>classes.tmain.layer.color='"+main_textcolor+"';classes.tmain.layer.paddingleft="+main_marginleft+";classes.tmain.layer.paddingtop="+main_margin_topbottom+";classes.tmain.layer.paddingbottom="+main_margin_topbottom+";classes.tmain.layer.fontsize='"+main_fontsize+"px';classes.tmain.layer.fontWeight='"+main_fontweight+"';classes.tmain.layer.textdecoration='"+main_textdecoration+"';classes.tmain.layer.fontfamily='"+main_fontfamily+"';";if(main_use_border)sw+="classes.tmain.layer.borderWidth=1;classes.tmain.layer.borderColor='"+main_border_color+"';classes.tmain.layer.borderStyle='solid';";sw+="classes.tmainhl.layer.color='"+main_hl_textcolor+"';classes.tmainhl.layer.paddingleft="+main_marginleft+";classes.tmainhl.layer.paddingtop="+main_margin_topbottom+";classes.tmainhl.layer.paddingbottom="+main_margin_topbottom+";classes.tmainhl.layer.fontsize='"+main_fontsize+"px';classes.tmainhl.layer.fontWeight='"+main_fontweight+"';classes.tmainhl.layer.textdecoration='"+main_hl_textdecoration+"';classes.tmainhl.layer.fontfamily='"+main_fontfamily+"';";if(main_use_border)sw+="classes.tmainhl.layer.borderWidth=1;classes.tmainhl.layer.borderColor='"+main_hl_border_color+"';classes.tmainhl.layer.borderStyle='solid';";sw+="classes.tsub.layer.color='"+sub_textcolor+"';classes.tsub.layer.paddingleft="+sub_marginleft+";classes.tsub.layer.paddingtop="+sub_margin_topbottom+";classes.tsub.layer.paddingbottom="+sub_margin_topbottom+";classes.tsub.layer.fontsize='"+sub_fontsize+"px';classes.tsub.layer.fontWeight='"+sub_fontweight+"';classes.tsub.layer.textdecoration='"+sub_textdecoration+"';classes.tsub.layer.fontfamily='"+sub_fontfamily+"';";if(sub_use_border)sw+="classes.tsub.layer.borderWidth=1;classes.tsub.layer.borderColor='"+sub_border_color+"';classes.tsub.layer.borderStyle='solid';";sw+="classes.tsubhl.layer.color='"+sub_hl_textcolor+"';classes.tsubhl.layer.paddingleft="+sub_marginleft+";classes.tsubhl.layer.paddingtop="+sub_margin_topbottom+";classes.tsubhl.layer.paddingbottom="+sub_margin_topbottom+";classes.tsubhl.layer.fontsize='"+sub_fontsize+"px';classes.tsubhl.layer.fontWeight='"+sub_fontweight+"';classes.tsubhl.layer.textdecoration='"+sub_hl_textdecoration+"';classes.tsubhl.layer.fontfamily='"+sub_fontfamily+"';";if(sub_use_border)sw+="classes.tsubhl.layer.borderWidth=1;classes.tsubhl.layer.borderColor='"+sub_hl_border_color+"';classes.tsubhl.layer.borderStyle='solid';";sw+="</STYLE>";document.writeln(sw);s56();;function s56(){tbid2="dtop";document.write("<layer id=ocblock z-index=100 width="+menu_width+" height="+menu_height+" visibility=show></layer>");for(var j=0;j<3;j++){if(j==1)tbid2="dbot";else  if(j==2)tbid2="off";maindiv="";(j==0)? tz_index=1:tz_index=2;if(j==2)tz_index=3;div_part=" z-index="+tz_index+" left=0 top=0 visibility='show'>";if((j==0)||(j==2)){document.write("<layer id='"+tbid2+"'"+div_part+"</layer>");}else {t_bs="<layer id=bcontainer visibility='show' width="+menu_width+">";document.write("<layer id='"+tbid2+"'"+div_part+t_bs+"</layer></layer>");}}document.close();};function s57(tbid){mdiv="";mdiv+="<layer id="+tbid+"main z-index=0 top="+0+" left="+0+" visibility='hide'";mdiv+=" bgcolor='"+background_color+"'";if(eval("window.main_background_image"))mdiv+=" background="+main_background_image;mdiv+=">";for(var i=0;i<mainCount;i++){iid=-1;tval=eval("window.icon_index"+i);if((tval)||(tval==0)){if(eval("window.icon_image"+tval)){iid=tval;(eval("window.icon_image_wh"+iid))? iiwh=s93(eval("icon_image_wh"+iid)):iiwh=new Array(10,10);(eval("window.icon_rollover_wh"+iid))? hliiwh=s93(eval("icon_rollover_wh"+iid)):hliiwh=iiwh;(eval("window.icon_rollover"+iid))? switchi=eval("window.icon_rollover"+iid):switchi=eval("window.icon_image"+iid);}}t_topborder=1;t_botborder=1;if(main_use_2pixel_topbottom_border){if(i==0)t_topborder=2;if(i==(mainCount-1))t_botborder=2;}mdiv+="<layer class='tmain' id="+tbid+"item"+i;if((eval("window.main_item_bgcolor"))&&(main_item_bgcolor.toLowerCase()!="transparent"))mdiv+=" bgcolor="+main_item_bgcolor+" ";mdiv+=">";if(main_use_border)mdiv+="<ilayer width="+(menu_width-8+main_ns4fix_rightborder_offset)+"></ilayer>";if(iid>-1)mdiv+="<img src='"+eval("icon_image"+iid)+"' width='"+iiwh[0]+"' height='"+iiwh[1]+"'>";mdiv+=eval("main_text"+i)+"</layer>";mdiv+="<layer class='tmainhl' id="+tbid+"hlitem"+i;if((eval("window.main_hl_item_bgcolor"))&&(main_hl_item_bgcolor.toLowerCase()!="transparent"))mdiv+=" bgcolor="+main_hl_item_bgcolor+" ";if(eval("window.main_hl_background_image"+i))mdiv+=" background="+eval("window.main_hl_background_image"+i);mdiv+=">";if(main_use_border)mdiv+="<ilayer width="+(menu_width-8+main_ns4fix_rightborder_offset)+"></ilayer>";if(iid>-1)mdiv+="<img src='"+switchi+"' width='"+iiwh[0]+"' height='"+iiwh[1]+"'>";tmt=eval("main_text"+i);if(eval("window.main_hl_text"+i))tmt=eval("main_hl_text"+i);mdiv+=tmt+"</layer>";}mdiv+="</layer>";return mdiv;};function s58(mindex,tbid,zind){var titem="";var subcount=0;while(eval("window.subdesc"+mindex+"_"+subcount))subcount++;var level=0;mindex=""+mindex;while((i=mindex.indexOf("_",i+1))>-1)level++;if(level==0)iw=sub1_indentwidth;else iw=sub1_indentwidth+sub2_indentwidth;mw=menu_width-iw;if(level>2)return;tli="level1";if(level==1)tli="level2";tsub+="<layer id="+tbid+"sub"+mindex+" z-index="+(zind+level)+" top="+0+" left="+0+" visibility='hide'";if(window.sub_background_color)tsub+=" bgcolor='"+sub_background_color+"'";else tsub+=" bgcolor='"+background_color+"'";if(eval("window.sub_"+tli+"_background_image"))tsub+=" background="+eval("sub_"+tli+"_background_image");tsub+=">";for(var i=0;i<subcount;i++){internalname=mindex+"_"+i;itemdesc=eval("subdesc"+internalname);hldesc=itemdesc;if(eval("window.hl_subdesc"+internalname))hldesc=eval("hl_subdesc"+internalname);if(eval("window.subdesc"+internalname+"_0")){var tvir=new Array(1);tvir[0]=internalname;s0=s0.concat(tvir);tvir[0]=false;s0_positioned=s0_positioned.concat(tvir);}iid=-1;tval=eval("window.icon_index"+mindex+"_"+i);if((tval)||(tval==0)){if(eval("window.icon_image"+tval)){iid=tval;(eval("window.icon_image_wh"+iid))? iiwh=s93(eval("icon_image_wh"+iid)):iiwh=new Array(10,10);(eval("window.icon_rollover_wh"+iid))? hliiwh=s93(eval("icon_rollover_wh"+iid)):hliiwh=iiwh;(eval("window.icon_rollover"+iid))? switchi=eval("window.icon_rollover"+iid):switchi=eval("window.icon_image"+iid);}}titem+="<layer class='tsub' id="+tbid+"item"+internalname+" left="+iw+" width="+mw+" z-index="+(zind+level);if((eval("window.sub_item_bgcolor"))&&(sub_item_bgcolor.toLowerCase()!="transparent"))titem+=" bgcolor='"+sub_item_bgcolor+"'";titem+=">";if(sub_use_border)titem+="<ilayer width="+(mw-8+sub_ns4fix_rightborder_offset)+"></ilayer>";if(iid>-1)titem+="<img src='"+eval("icon_image"+iid)+"' width='"+iiwh[0]+"' height='"+iiwh[1]+"'>";titem+=itemdesc+"</layer>";titem+="<layer class='tsubhl' id="+tbid+"hlitem"+internalname+" left="+iw+" width="+mw+" z-index="+(zind+level);if((eval("window.sub_hl_item_bgcolor"))&&(sub_hl_item_bgcolor.toLowerCase()!="transparent"))titem+=" bgcolor='"+sub_hl_item_bgcolor+"'";titem+=">";if(sub_use_border)titem+="<ilayer width="+(mw-8+sub_ns4fix_rightborder_offset)+"></ilayer>";if(iid>-1)titem+="<img src='"+switchi+"' width='"+iiwh[0]+"' height='"+iiwh[1]+"'>";titem+=hldesc+"</layer>";}tsub+=titem;tsub+="</layer>";};function s59(){if((mac)&&(s41))return;treetop=s63("","dtop");treebot=s63("","dbot");treeoff=s63("","off");tspace=s63("","tree_space");block=s63("","ocblock");s6=tspace.pageX;s7=tspace.pageY;treetop.document.write(s78(0)+tsub);treetop.document.close();treetop.left=s6;treetop.top=s7;treetop.clip.width=menu_width;treetop.clip.height=menu_height;treebot.document.bcontainer.document.writeln(s78(1)+tsub);treebot.document.close();treebot.left=s6;treebot.top=s7;treebot.clip.width=menu_width;treebot.clip.height=menu_height;treeoff.document.writeln(s78(2)+tsub);treeoff.document.close();treeoff.left=s6;treeoff.top=s7;treeoff.clip.width=menu_width;treeoff.clip.height=menu_height;block.left=s6;block.top=s7;s79();if(start_open_index.toLowerCase()!="none")s47=setInterval("s96()",100);s41=true;if(!s80())s91();if((window.onload_statement)&&(window.onload_statement!=null))eval(onload_statement);};function s61(){subtop=findNetscapeItem(document.dtop.layers,"tmain");subbot=findNetscapeItem(document.dbot.layers[0].layers,"bmain");suboff=findNetscapeItem(document.off.layers,"omain");var cursub=subtop;for(j=0;j<3;j++){ah=0;if(j==1)cursub=subbot;else  if(j==2)cursub=suboff;subchildren=cursub.layers;tml=subchildren.length;s4=new Array(tml/2);main_y=new Array(tml/2);s5=new Array(tml/2);if(j==0)s27=new Array(tml/2);else  if(j==1)s28=new Array(tml/2);s29=new Array(tml/2);s30=new Array(tml/2);s31=new Array(tml/2);mi=0;for(var i=0;i<tml;i=i+2){if((main_itemheight!="auto")&&(main_itemheight>5)){subchildren[i].clip.height=main_itemheight;subchildren[i+1].clip.height=main_itemheight;}subchildren[i].left=0;subchildren[i].top=ah;subchildren[i].clip.width=menu_width;subchildren[i+1].visibility="hide";subchildren[i+1].left=0;subchildren[i+1].top=ah;subchildren[i+1].clip.width=menu_width;s4[mi]=subchildren[i].clip.height;s5[mi]=false;if(j==0)s27[mi]=subchildren[i+1];else  if(j==1)s28[mi]=subchildren[i+1];s29[mi]=mi+"";s30[mi]=subchildren[i].pageY;s31[mi]=subchildren[i].clip.height;ah+=subchildren[i].clip.height;mi++;}cursub.clip.height=ah+main_extend_height;cursub.clip.width=menu_width;if(j==2)cursub.visibility="hide";else cursub.visibility="show";}};function findNetscapeItem(layerobj,index){for(ii=0;ii<layerobj.length;ii++){if(layerobj[ii].id==index)return layerobj[ii];}return null;};function s62(index){treetop=s63("","dtop");treebot=s63("","dbot");treeoff=s63("","off");subtop=findNetscapeItem(treetop.layers,"tsub"+index);subbot=findNetscapeItem(treebot.layers[0].layers,"bsub"+index);suboff=findNetscapeItem(treeoff.layers,"osub"+index);level=1;index=""+index;while((i=index.indexOf("_",i+1))>-1)level++;var cursub=subtop;for(j=0;j<3;j++){ah=0;if(j==1)cursub=subbot;else  if(j==2)cursub=suboff;subchildren=cursub.layers;cursub.numsubs=subchildren.length;for(var i=0;i<subchildren.length;i=i+2){if((sub_itemheight!="auto")&&(sub_itemheight>5)){subchildren[i].clip.height=sub_itemheight;subchildren[i+1].clip.height=sub_itemheight;}subchildren[i].top=ah;subchildren[i+1].visibility="hide";subchildren[i+1].top=ah;ah+=subchildren[i].clip.height;}cursub.visibility="hide";cursub.clip.width=menu_width;cursub.left=0;cursub.clip.height=ah;}};function s63(index,topbot){subid=eval("document."+topbot+index);return subid;};function s68(e){if(s43){alert("Browser Error - To reinitialize the menu click the 'Reload' button.");s43=false;}if(!s41)return;if(s32!="none"){itid=s28[s32].id;itid=itid.substring(7);if(itid.indexOf("_")==-1){s81(null,itid);}else  if(itid.indexOf("_")>-1){s82(null,itid);}}};function s69(index){if(s11!=null)return;coords=null;bmitem=findNetscapeItem(document.dtop.layers,"tmain");tobj=findNetscapeItem(bmitem.layers,"titem"+index);coords=new Object();coords.y=tobj.pageY;coords.x=tobj.pageX;ai=coords.y+s4[index];clip_h=ai-s7;botmain=findNetscapeItem(document.dbot.layers[0].layers,"bmain");if((eval("window.subdesc"+index+"_0"))&&(!s5[index])){if(s16!=null)s16.visibility="hide";tsub=findNetscapeItem(document.dbot.layers[0].layers,"bsub"+index);tsub.top=-parseInt(tsub.clip.height);document.dbot.layers[0].clip.top=-parseInt(tsub.clip.height);document.dbot.layers[0].clip.height=parseInt(tsub.clip.height)+parseInt(botmain.clip.height);tsub.visibility="show";ttsub=findNetscapeItem(document.dtop.layers,"tsub"+index);ttsub.top=clip_h;s16=tsub;treebot=s63("","dbot");nmh=menu_height-(ai-s7);treebot.top=ai;treebot.clip.height=nmh;s10=parseInt(tsub.clip.height);botmain.top=-clip_h;s86(true);s5[index]=true;s17=index;msize=s4.length;ssize=(tsub.numsubs/2);as=msize+ssize;s27=new Array(as);s28=new Array(as);s29=new Array(as);s30=new Array(as);s31=new Array(as);addh=0;subh=parseInt(ttsub.clip.height);substart=0;for(i=0;i<msize;i++){s27[i]=bmitem.layers[(i*2)+1];s28[i]=botmain.layers[(i*2)+1];s29[i]=i+"";s30[i]=s7+addh;s31[i]=s4[i];if(i>index)s30[i]+=subh;else substart=s30[i]+s31[i]-s7;addh+=s4[i];}addh=substart;for(i=msize;i<(ssize+msize);i=i+1){sind=index+"_"+(i-msize);mi=findNetscapeItem(ttsub.layers,"titem"+sind);tobj=findNetscapeItem(ttsub.layers,"thlitem"+sind);s27[i]=tobj;tobj=findNetscapeItem(tsub.layers,"bhlitem"+sind);s28[i]=tobj;s29[i]=sind;s30[i]=s7+addh;s31[i]=parseInt(mi.clip.height);addh+=parseInt(mi.clip.height);}}else  if(s5[index]){bsub=findNetscapeItem(document.dbot.layers[0].layers,"bsub"+index);s8=parseInt(bsub.clip.height);osub=findNetscapeItem(document.off.layers,"osub"+index);osub.top=0;offmain=findNetscapeItem(document.off.layers,"omain");offmain.top=parseInt(osub.clip.height)-clip_h;document.off.top=ai;nmh=menu_height-(ai-s7);document.off.clip.height=nmh;osub.visibility="show";offmain.visibility="show";tsub=findNetscapeItem(document.dtop.layers,"tsub"+index);tsub.visibility="hide";tsub=bsub;tsub.visibility="show";tsub.top=-parseInt(tsub.clip.height);document.dbot.top=ai;document.dbot.clip.height=nmh;if(s15!=null)s15.visibility="hide";botmain.top=-clip_h;document.dbot.layers[0].top=s8;offmain.visibility="hide";osub.visibility="hide";s86(false);s5[index]=false;s17="none";msize=s4.length;s29=new Array(msize);s30=new Array(msize);s31=new Array(msize);addh=0;for(i=0;i<msize;i++){s29[i]=i+"";s30[i]=s7+addh;s31[i]=s4[i];addh+=s4[i];}}};function s70(index){if(eval("window.subdesc"+index+"_0")){tb_sub=findNetscapeItem(document.dbot.layers[0].layers,"bsub"+index);tb_sub2=findNetscapeItem(document.dtop.layers,"tsub"+index);top_callsub=findNetscapeItem(document.dtop.layers,"tsub"+index.substring(0,index.lastIndexOf("_")));bottom_callsub=findNetscapeItem(document.dbot.layers[0].layers,"bsub"+index.substring(0,index.lastIndexOf("_")));botmain=findNetscapeItem(document.dbot.layers[0].layers,"bmain");tep=index.substring(index.lastIndexOf("_")+1);tbp=index.substring(0,index.lastIndexOf("_"));mindex=index.substring(0,index.indexOf("_"));if(tb_sub.expanded){s8=parseInt(document.dbot.layers[0].top);document.dbot.layers[0].clip.top=-parseInt(tb_sub.clip.height)-parseInt(botmain.clip.height)-parseInt(bottom_callsub.clip.height);tb_sub.expanded=false;s86(false);s18="none";msize=s4.length;ssize=(top_callsub.numsubs/2);array_size=msize+ssize;s29=new Array(array_size);s30=new Array(array_size);s31=new Array(array_size);addh=0;subh=parseInt(top_callsub.clip.height);substart=0;s27=new Array(as);s28=new Array(as);for(i=0;i<msize;i++){s29[i]=i+"";s30[i]=s7+addh;s31[i]=s4[i];tobj=findNetscapeItem(document.dtop.layers,"tmain");s27[i]=tobj.layers[(i*2)+1];s28[i]=botmain.layers[(i*2)+1];if(i>mindex)s30[i]+=subh;else substart=s30[i]+s31[i]-s7;addh+=s4[i];}addh=substart;for(i=msize;i<ssize+msize;i++){sind=mindex+"_"+(i-msize);mi=findNetscapeItem(top_callsub.layers,"titem"+sind);tobj=findNetscapeItem(top_callsub.layers,"thlitem"+sind);s27[i]=tobj;tobj=findNetscapeItem(bottom_callsub.layers,"bhlitem"+sind);s28[i]=tobj;s29[i]=sind;s30[i]=s7+addh;s31[i]=parseInt(mi.clip.height);addh+=parseInt(mi.clip.height);}}else {tc=new Object();tc.y=bottom_callsub.pageY;tc.x=bottom_callsub.pageX;top_callsub.visibility="show";top_callsub.top=tc.y-s7;if(s15!=null)s15.visibility="hide";tb_sub.visibility="hidden";tb_sub.top=-parseInt(tb_sub.clip.height);tb_sub2.top=-parseInt(tb_sub.clip.height);s15=tb_sub;s10=parseInt(tb_sub.clip.height)+s8;tc=1;bheight=0;while(findNetscapeItem(bottom_callsub.layers,"bitem"+tbp+"_"+(new Number(tep)+tc))!=null){tbheight_obj=findNetscapeItem(bottom_callsub.layers,"bitem"+tbp+"_"+(new Number(tep)+tc));bheight+=parseInt(tbheight_obj.clip.height);tc++;}topmain=findNetscapeItem(document.dtop.layers,"tmain");topmain_item=findNetscapeItem(topmain.layers,"titem"+mindex);tof=parseInt(topmain_item.top)+parseInt(topmain_item.clip.height);coords=new Object();coords.y=top_callsub.pageY;coords.x=top_callsub.pageX;offmain=findNetscapeItem(document.off.layers,"omain");if(bheight>0){offmain.visibility="show";offmain.top=-parseInt(tof);document.off.top=coords.y+parseInt(top_callsub.clip.height);}tcontain=document.dbot.layers[0];tcontain.top=-menu_height;tcontain.clip.top=-parseInt(tb_sub.clip.height);tcontain.clip.height=parseInt(tb_sub.clip.height)+parseInt(botmain.clip.height)+parseInt(bottom_callsub.clip.height);botmain.top=-parseInt(tof)+bheight;bottom_callsub.visibility="show";bottom_callsub.top=bheight-parseInt(bottom_callsub.clip.height);document.dbot.top=coords.y+parseInt(top_callsub.clip.height)-bheight;nmh=menu_height-(document.dbot.top-s7);document.dbot.clip.height=nmh;tcontain.top=s8;tb_sub.visibility="show";if(bheight>0){offmain.visibility="hide";}tb_sub.expanded=true;s86(true);s18=index;msize=s4.length;s1_size=(top_callsub.numsubs/2);s2_size=(tb_sub.numsubs/2);array_size=msize+s1_size+s2_size;s29=new Array(array_size);s30=new Array(array_size);s31=new Array(array_size);s27=new Array(array_size);s28=new Array(array_size);addh=0;subh1=parseInt(top_callsub.clip.height);subh2=parseInt(tb_sub.clip.height);substart=0;for(i=0;i<msize;i++){s29[i]=i+"";s30[i]=s7+addh;s31[i]=s4[i];mi=s63(i,"titem");s27[i]=topmain.layers[(i*2)+1];s28[i]=botmain.layers[(i*2)+1];if(i>mindex)s30[i]+=subh1+subh2;else substart=s30[i]+s31[i]-s7;addh+=s4[i];}addh=substart;for(i=msize;i<s1_size+msize;i++){sind=mindex+"_"+(i-msize);mi=findNetscapeItem(top_callsub.layers,"titem"+sind);tobj=findNetscapeItem(top_callsub.layers,"thlitem"+sind);s27[i]=tobj;tobj=findNetscapeItem(bottom_callsub.layers,"bhlitem"+sind);s28[i]=tobj;s29[i]=sind;s30[i]=s7+addh;s31[i]=parseInt(mi.clip.height);if((i-msize)>tep)s30[i]+=subh2;else substart=s30[i]+s31[i]-s7;addh+=parseInt(mi.clip.height);}addh=substart;for(i=msize+s1_size;i<s1_size+s2_size+msize;i++){sind=index+"_"+(i-msize-s1_size);mi=findNetscapeItem(tb_sub2.layers,"titem"+sind);tobj=findNetscapeItem(tb_sub2.layers,"thlitem"+sind);s27[i]=tobj;tobj=findNetscapeItem(tb_sub.layers,"bhlitem"+sind);s28[i]=tobj;s29[i]=sind;s30[i]=s7+addh;s31[i]=parseInt(mi.clip.height);addh+=parseInt(mi.clip.height);}}}};function s71(){s8+=scrolljump;if(s8>s10)s8=s10;document.dbot.layers[0].top=s8;if(s8>=s10)s87();};function s72(){s8 -=scrolljump;if(s8<0)s8=0;document.dbot.layers[0].top=s8;if(s8<=0)s87();};function hl(i,bottomonly){if(s32!=i){s74();if(!bottomonly){obj=s27[i];obj.visibility="show";}obj=s28[i];obj.visibility="show";s32=i;ti=s28[i].id.substring(7);s94(ti);}};function s74(){if(s32!="none"){obj=s27[s32];obj.visibility="hide";obj=s28[s32];obj.visibility="hide";ti=s28[s32].id.substring(7);s95(ti);s32="none";}};function s77(){if(ns4){if((window.onresize_statement)&&(window.onresize_statement!=null))eval(onresize_statement);}s43=true;}
