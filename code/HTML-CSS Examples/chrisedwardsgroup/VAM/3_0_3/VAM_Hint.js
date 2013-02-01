// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


function VAM_InitHint(pFId, pHO){var vF = VAM_GetById(pFId);vF.Hint = pHO;VAM_AttachEvent(vF, "onfocus", "VAM_ShowHint('" + pFId + "');", false);VAM_AttachEvent(vF, "onblur", "VAM_HideHint('" + pFId + "');", false);if (pHO.ID){pHO.Pnl = VAM_GetById(pHO.ID);pHO.Lbl = VAM_GetById(pHO.ID + "_Text");if (pHO.Lbl == null) 
pHO.Lbl = pHO.Pnl;pHO.Pnl.OrigCss = pHO.Pnl.className;if (pHO.Md){if (pHO.Lbl.innerHTML) 
VAM_SetInnerHTML(pHO.Lbl, "");pHO.SD = pHO.Pnl.style.display; if (pHO.SD == "none") 
pHO.SD = "block";pHO.Pnl.style.visibility = "hidden"; if (pHO.Md == 2)pHO.Pnl.style.display = "none";}}} 

var gVAM_HintCnt = -1;function VAM_ShowHint(pFId){var vF = VAM_GetById(pFId);var vHO = vF.Hint;if (!vHO) 
return;if (vHO.SB && (window.status != null))window.status = VAM_StripTags(vHO.H);var vErr = VAM_IsValid(vF) == false; if (vHO.E && vErr)return;if (vHO.Pnl)vHO.Pnl.className = vHO.Pnl.OrigCss;var vH = vHO.H; if (vErr && window.gVAM_HSE) 
{var vM = ""; var vA = vF.ActionIDs;if (vA){gVAM_HintCnt++; for (var vI = 0; vI < vA.length; vI++){var vAO = gVAMActions[vA[vI]];if ((vAO.VT == "VAL") && !vAO.IsValid && (vAO.HintCnt != gVAM_HintCnt)) 
{var vMsg = VAM_GetErrMsg(vAO);if (vMsg != ""){if (vM != "") 
vM = vM + gVAM_HSESep;vM = vM + vMsg;if ((gVAM_HSE == 1) || (gVAM_HSE == 3)) 
break;}vAO.HintCnt = gVAM_HintCnt; }} } 
if (vM != "") 
{if (gVAM_HSECss2 != "") 
vM = "<span class='" + gVAM_HSECss2 + "'>" + vM + "</span>";if (gVAM_HSE > 2) 
vH = vM;elsevH = vM + gVAM_HSESep + vH;if (vHO.Pnl && (gVAM_HSECss != ""))vHO.Pnl.className = gVAM_HSECss;}} if (vH == "") 
return;if (vHO.CH) 
if (vHO.CH(vF,true,vH,vHO.Pnl)) 
{VAM_FixAbsPos(true);return;}if (vHO.Md && vHO.Lbl) 
{VAM_SetInnerHTML(vHO.Lbl, vH);vHO.Pnl.style.visibility = "inherit";vHO.Pnl.style.display = vHO.SD;VAM_FixAbsPos(true);}} 

function VAM_HideHint(pFId){var vF = VAM_GetById(pFId);var vHO = vF.Hint;if (!vHO) 
return;if (vHO.SB && (window.status != null))window.status = "";if (vHO.CH) 
if (vHO.CH(vF,false,"",vHO.Pnl)) 
{VAM_FixAbsPos(true);return;}if (vHO.Md && vHO.Lbl){VAM_SetInnerHTML(vHO.Lbl, "");vHO.Pnl.style.visibility = "hidden";if (vHO.Md == 2)vHO.Pnl.style.display = "none";VAM_FixAbsPos(true);}} 

function VAM_ChgHint(pFId, pHint){var vF = VAM_GetById(pFId);var vHO = vF.Hint;if (vHO){var vOld = vF.Hint.H;vF.Hint.H = pHint;return vOld;}return null;} 
