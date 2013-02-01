// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


function VAM_ReformatInit(pAO){var vFld = VAM_GetById(pAO.Cond.IDToEval);if (vFld == null){pAO.Enabled = false;return;}VAM_InitSOC(pAO.Cond.IDToEval, pAO.DAC); } 

function VAM_ReformatAction(pAO, pEvalRes){var vFld = VAM_GetById(pAO.Cond.IDToEval);if (pEvalRes == 1){var vText = pAO.ToStrFnc(pAO, pAO.Cond.Val);if (vText != null){vFld.value = vText;}}} 

function VAM_DateFmt(pAO, pVal){var vR = pAO.DateTmpt;var vM = pVal.getMonth() + 1;var vD = pVal.getDay();var vY = pVal.getFullYear();if (vR.indexOf("MM") > -1)vR = vR.replace("MM", (vM > 9) ? vM : "0" + vM.toString());elsevR = vR.replace("M", vM);if (vR.indexOf("DD") > -1)vR = vR.replace("DD", (vD > 9) ? vD : "0" + vD.toString());elsevR = vR.replace("D", vD);if (vR.indexOf("YYYY") > -1)vR = vR.replace("YYYY", vY);elsevR = vR.replace("YY", vY % 100);return vR;} 

function VAM_CurrFmt(pAO, pVal){
var vCurr = Math.abs(pVal).toString(); if (pAO.mindecdig > 0) 
{var vPos = vCurr.indexOf(".");if ((vPos > -1) || !pAO.hidezero) 
{var vNeeded = (vPos == -1) ? pAO.mindecdig : (pAO.mindecdig - (vCurr.length - vPos - 1));if (vPos == -1)vCurr = vCurr + ".";if (vNeeded > 0)vCurr = vCurr + String("00000000").substr(0, vNeeded);}}
if (pAO.Cond.decsep != ".")vCurr = vCurr.replace(".", pAO.Cond.decsep);vCurr = VAM_AddGrpSep(pAO, vCurr);if (pVal >= 0)return pAO.PosTmpt.replace("{0}", vCurr);elsereturn pAO.NegTmpt.replace("{0}", vCurr);} 

function VAM_IntFmt(pAO, pVal){var vR = Math.abs(pVal).toString();vR = VAM_AddGrpSep(pAO, vR);if (vR.length < pAO.FLZ){vT = "0000000000";vR = vT.substr(0, pAO.FLZ - vR.length) + vR;}if (pVal < 0)vR = pAO.NegTmpt.replace("{0}", vR);return vR;} 

function VAM_DecFmt(pAO, pVal){var vR = Math.abs(pVal).toString();if ((pAO.trzero != 0) && (vR.indexOf(".") == -1))vR = vR + ".0"; if (pAO.trzero > 0){var vN = pAO.trzero - (vR.length - vR.indexOf(".") - 1);if (vN > 0)vR = vR + String("000000000000000000").substr(0, vN);}if (pAO.Cond.decsep != ".")vR = vR.replace(".", pAO.Cond.decsep);vR = VAM_AddGrpSep(pAO, vR);if (pVal < 0)vR = pAO.NegTmpt.replace("{0}", vR);return vR;} 

function VAM_AddGrpSep(pAO, pText){if (!pAO.Cond.ShowTS)return pText;var vDecPos = pText.indexOf(pAO.Cond.decsep);var vR = "";var vEndPos = pText.length;if (vDecPos > -1) 
{vR = pText.substring(vDecPos, vEndPos);vEndPos = vDecPos;}vDecPos = vEndPos - 3;while (vEndPos > 3){if (vDecPos < 0)vDecPos = 0;vR = pAO.Cond.grpsep + pText.substring(vDecPos, vEndPos) + vR;vDecPos = vDecPos - 3;vEndPos = vEndPos - 3;}if (vEndPos > 0)vR = pText.substring(0, vEndPos) + vR;return vR;} 

function VAM_GetDTTBValue(pTBId){var vR = null;var vAO = VAM_FindAOById(pTBId);var vFld = VAM_GetById(pTBId);if ((vAO != null) && (vFld != null)){if (VAM_EvalCondition(vAO.Cond) == 1)vR = vAO.Cond.Val;}return vR;} 

function VAM_SetDTTBValue(pTBId, pValue){var vR = false;var vText = VAM_FormatDTTBValue(pTBId, pValue);if (vText != null){var vFld = VAM_GetById(pTBId);vFld.value = vText;if (vFld.orval == null) 
vFld.orval = ""; VAM_SOCCheck(pTBId); vR = true;}return vR;}
function VAM_FormatDTTBValue(pTBId, pValue){var vR = null;var vAO = VAM_FindAOById(pTBId);if (vAO != null){vR = vAO.ToStrFnc(vAO, pValue);}return vR;}
var gVAM_PassThruKey = false; var gVAM_KFVal = null;function VAM_InitKey(pTBId, pKO){var vFld = VAM_GetById(pTBId);vFld.KO = pKO;} 

function VAM_KeyPress(pFld, pE){if (gVAM_PassThruKey) 
return true;var vR = false;if (pFld.disabled || pFld.readOnly)return false;if (VAM_IsCtrl(pE)) 
return true;var vKO = pFld.KO;if (!vKO) 
{
return false;}
if (gVAM_KFVal && vKO.MxTab){VAM_TabAtMax(pFld);if (!gVAM_KFVal){VAM_StopEvent(pE);return false;}}var vKC = VAM_GetKeyCode(pE);if ((vKC == null) || (vKC == 0) || 
(vKC == 57401) || 
(gIsSafari && (vKC >= 63232) && (vKC <= 63276))) 
return true;if (vKO.CstmKP && !vKO.CstmKP(pFld, pE, vKC)){VAM_StopEvent(pE);return false;}var vKCS = String.fromCharCode(vKC); if (vKC == 13) 
{
if (vKO.EBtn){
VAM_ClickBtn(vKO.EBtn, true);}
else if (vKO.ETab)VAM_SetFocus(vKO.NxtId); elsevR = vKO.Ent ? !vKO.Exc : false;}else if (vKC < 30) 
vR = true;else 
{
if (vKO.CTab && (vKO.CTab.indexOf(vKCS) > -1) && (pFld.value != ""))VAM_SetFocus(vKO.NxtId); else if (vKO.Fltr == "") 
vR = true;else if (vKO.Fltr.indexOf(vKCS) > -1)vR = !vKO.Exc;elsevR = vKO.Exc;}if (vR){if ((vKC >= 30)&&(pFld.maxLength)) 
gVAM_KFVal = pFld.value;}elseVAM_StopEvent(pE);return vR;} 

function VAM_OnKeyDown(pFld, pE){if (!pFld.KO) 
{alert('Page is loading. Please wait.');VAM_StopEvent(pE);return false;}gVAM_PassThruKey = false;var vKC = VAM_GetKeyCode(pE);if (pFld.KO.CstmKD && !pFld.KO.CstmKD(pFld, pE, vKC)){VAM_StopEvent(pE);return false;}if (gIsSafari) 
return true;var vKCStr = String.fromCharCode(vKC);if ((vKC >= 33) && (vKC <= 47))gVAM_PassThruKey = true;return true;} 

function VAM_TabAtMax(pFld){if ((gVAM_KFVal != null) && 
(pFld.value.length == pFld.maxLength) && 
(pFld.value.length > gVAM_KFVal.length) && 
(pFld.value.substr(0, gVAM_KFVal.length) == gVAM_KFVal)) 
{VAM_SetFocus(pFld.KO.NxtId);gVAM_KFVal = null; }return true;} 

function VAM_ClickBtn(pBId, pFcs){var vB = VAM_GetById(pBId);if ((vB.disabled == null) || !vB.disabled)if (vB.click){if (pFcs && vB.focus)vB.focus(); vB.click();}} 

function VAM_KeyToBtn(pE, pKC, pBId, pFcs){var vKC = VAM_GetKeyCode(pE);if (vKC == pKC){VAM_ClickBtn(pBId, pFcs);VAM_StopEvent(pE);return false;}return true;} 
function VAM_NoPaste(pTBId){var vFld = VAM_GetById(pTBId);vFld.onpaste = new Function("event.returnValue=false;return false;");} 

function VAM_GetKeyCode(pE){var vKeyCode = null;if (pE.keyCode)vKeyCode = pE.keyCode;else if (pE.which)vKeyCode = pE.which;return vKeyCode; } 
function VAM_StopEvent(pE){if (pE.cancelBubble != null)pE.cancelBubble = true;if (pE.stopPropagation)pE.stopPropagation();if (pE.preventDefault)pE.preventDefault();if (pE.returnValue != null)pE.returnValue = false;if (pE.cancel != null)pE.cancel = true;} 

function VAM_IsCtrl(pE){var vCtrlKey = false;if (pE.ctrlKey != null)vCtrlKey = pE.ctrlKeyelse if (pE.modifiers != null)vCtrlKey = (pE.modifiers | 2) != 0;return vCtrlKey;} 

function VAM_InitSOC(pFId, pDAC){var vFld = VAM_GetById(pFId);if (vFld.value != null) 
{VAM_AttachEvent(vFld, "onblur", "VAM_SOCCheck('" + pFId + "');");VAM_AttachEvent(vFld, "onchange", "VAM_SOCSet('" + pFId + "','C');");VAM_AttachEvent(vFld, "onfocus", "VAM_SOCSet('" + pFId + "','F');");if (pDAC)vFld.autocomplete = "off"; }} 

function VAM_SOCCheck(pTBId){var vFld = VAM_GetById(pTBId);var vChg = null; if ((vFld.orval != null) && (vFld.orval != vFld.value)){vFld.orval = vFld.value;VAM_FireEvent(vFld, "change", "HTMLEvents");vChg = true;}VAM_GOCSet(vFld, false, vChg); } 

function VAM_SOCSet(pTBId, pEvt){var vFld = VAM_GetById(pTBId);if (VAM_TBIsBlank(vFld))vFld.orval = "";else
vFld.orval = vFld.value;if (pEvt == 'F')VAM_GOCSet(vFld, true, null);else if (pEvt == 'C')VAM_GOCSet(vFld, false, true);} 

function VAM_GOCInit(pCFld, pDEFld, pOnC){if (!pDEFld.gocfld) 
{pCFld.HFoc = null; pCFld.Dirty = false;pDEFld.gocfld = pCFld;if (!pCFld.gocDE)pCFld.gocDE = new Array;pCFld.gocDE[pCFld.gocDE.length] = pDEFld;}} 

function VAM_GOCInitCstmOC(pCFID, pOnC){VAM_GetById(pCFID).soconchange = pOnC;} 

function VAM_GOCCheck(pCFID){var vFld = VAM_GetById(pCFID);if (vFld.HFoc == null && vFld.Dirty){if (vFld.ActionIDs) 
VAM_FieldChanged(pCFID);vFld.Dirty = false; if (vFld.soconchange)vFld.soconchange();}} 

function VAM_GOCSet(pDEFld, pHF, pD){var vFld = pDEFld.gocfld;if (vFld){vFld.HFoc = pHF ? pDEFld : null;if (pD != null)vFld.Dirty = pD;if (!pHF && vFld.Dirty)window.setTimeout("VAM_GOCCheck('" + vFld.id + "');", 20);}} 

function VAM_VWBInit(){for (var vI = 0; vI < gVAM_VWB.length; vI++){var vO = gVAM_VWB[vI]; var vF = VAM_GetById(vO.CID);vF.VWB = vO.VWB;vF.VWBUC = vF.VWB.toUpperCase();vF.VWBCss = vO.Css;if (gVAM_VWBMd != 0) 
VAM_AttachEvent(vF, "onfocus", "VAM_VWBClear('" + vO.CID + "');");VAM_AttachEvent(vF, "onblur", "VAM_VWBSet('" + vO.CID + "');");if (vF.OrigCss == null){
if (vF.style.OrigCss != null)vF.OrigCss = vF.style.OrigCss;else
vF.OrigCss = vF.className;}
VAM_VWBSet(vO.CID);}} 

function VAM_VWBClear(pFId){var vF = VAM_GetById(pFId);if (vF.value.toUpperCase() == vF.VWBUC) 
{vF.value = "";if (gVAM_VWBMd == 2)VAM_VWBFixCss(vF);vF.select(); }} 

function VAM_VWBSet(pFId){var vF = VAM_GetById(pFId);var vVal = vF.value.toUpperCase();var vCss = false; if (vVal != vF.VWBUC) 
{if (vVal == "") 
{vF.value = vF.VWB;vCss = true;}else 
VAM_VWBFixCss(vF);}else if ((vF.VWB == "") && (vVal == "")) 
vCss = true;else if ((vVal == vF.VWBUC) && (gVAM_VWBMd == 2)) 
vCss = true;if (vCss && (vF.VWBCss != "") && (window.gVAM_ErrCtlCss != vF.className)) 
vF.className = vF.VWBCss;} 

function VAM_VWBFixCss(pF){if (pF.VWBCss != "") 
{if ((window.gVAM_ErrCtlCss == null) || (VAM_IsValid(pF) != false)) 
pF.className = pF.OrigCss;}} 

function VAM_TBIsBlank(pF){if (pF.value == "")return true;else if (pF.VWB == null)return false;return pF.value.toUpperCase() == pF.VWBUC; } 

function VAM_FireEvent(pFld, pEN, pDOMET){if (typeof(pFld) == "string") 
pFld = VAM_GetById(pFld);if (pFld.fireEvent != null) 
pFld.fireEvent('on'+pEN);else if ((document.createEvent != null) && !gIsOpera7) 
{var vEv = document.createEvent(pDOMET);switch (pDOMET){case "TextEvents":vEv.initTextEvent(pEN, true, false);break;case "UIEvents":vEv.initUIEvent(pEN, true, false);break;case "MouseEvents":vEv.initMouseEvent(pEN, true, false);break;default: 
vEv.initEvent(pEN, true, false);break;}pFld.dispatchEvent(vEv);}else 
{
var vEv = "";if (gIsOpera7)vEv = eval("pFld.on" + pEN);elsevEv = pFld.getAttribute('on'+pEN);vEv = vEv.toString();vEv = vEv.substring(vEv.indexOf("{") + 1, vEv.lastIndexOf("}"));eval(vEv + ';');}} 
