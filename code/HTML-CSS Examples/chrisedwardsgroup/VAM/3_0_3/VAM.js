// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


var gVAM_UA = navigator.userAgent.toLowerCase();var gVAM_OS, gVAM_Browser, gVAM_Version, gVAM_place, gVAM_Temp;var gIsIEWin = false;var gIsIEWin55 = false;var gIsIEMac = false;var gIsIEMac51 = false;var gIsNetscapeNav = false;var gIsNetscapeMoz = false; var gIsSafari = false;var gIsOpera = false; var gIsOpera7 = false; var gSupportsInnerHTML = (document.body != null) && (document.body.innerHTML != null);var gSupportsSetInterval = window.setInterval != null;if (checkIt('msie')) {gVAM_Browser = "Internet Explorer"; gIsIEWin = true; }else if (checkIt('safari') || checkIt('applewebkit')) {gVAM_Browser = "Safari"; gIsSafari = true; } 
else if (checkIt('opera')) {gVAM_Browser = "Opera"; gIsOpera = true; } 
else if (checkIt('netscape')) {gVAM_Browser = "Netscape Mozilla"; gIsNetscapeMoz = true; }else if (checkIt('gecko')) {gVAM_Browser = "Gecko"; gIsNetscapeMoz = true; } 
else if (!checkIt('compatible')){gVAM_Browser = "Netscape Navigator"gVAM_Version = gVAM_UA.charAt(8);gIsNetscapeNav = true;}else gVAM_Browser = "Unknown";if (!gVAM_Version && gVAM_place) gVAM_Version = gVAM_UA.charAt(gVAM_place + gVAM_Temp.length);if (!gVAM_OS){if (checkIt('linux')) gVAM_OS = "Linux";else if (checkIt('x11')) gVAM_OS = "Unix";else if (checkIt('mac')) gVAM_OS = "Mac"else if (checkIt('win')) gVAM_OS = "Windows"else gVAM_OS = "Unknown";}if (gIsIEWin && (gVAM_OS == "Mac")){gIsIEWin = false;gIsIEMac = true;if (gVAM_UA.indexOf("msie 5.1") > -1)gIsIEMac51 = true;}else if (gIsIEWin){if ((gVAM_UA.indexOf("msie 5.5") > -1) || (gVAM_UA.indexOf("msie 6") > -1))gIsIEWin55 = true;}
else if (gIsOpera){if (parseInt(gVAM_Version) >= 7){gIsOpera7 = true;gIsOpera = false;}}var gSupportsMultilineTT = gIsIEWin || gIsIEMac51; function checkIt(string){gVAM_place = gVAM_UA.indexOf(string) + 1;gVAM_Temp = string;return gVAM_place;}
var gVAM_MAId = null;var gVAM_Init = true; var gVAM_AONoIDs = new Array;function VAM_GetById(pId){if (document.getElementById)return document.getElementById(pId);else if (document.all)return document.all[pId]else if (document.layers) 
{var vElement = "";eval("if (document."+ pId + ") vElement = document."+pId+"; else vElement =document." + gVAM_FormName + "." + pId);return vElement;}elsereturn null; } 

function VAM_GetAtt(pE, pAName, pDefVal){if (pE.getAttribute){var vR = pE.getAttribute(pAName, 0);if (vR == null)vR = pDefVal;else if ((vR == "") && (!document.all)) 
vR = pDefVal;return vR;}else 
return pDefVal;} 

function VAM_SetInnerHTML(pFld, pValue){if (gIsIEMac51){pFld.innerHTML = "";var vNewEl = document.createElement("span");vNewEl.innerHTML = pValue;pFld.appendChild(vNewEl);}elsepFld.innerHTML = pValue;} 
function VAM_SetLeftPos(pFld, pPos){if (pFld.style.pixelLeft)pFld.style.pixelLeft = pPos;else if (pFld.style.posLeft)pFld.style.posLeft = pPos.toString() + "px";elsepFld.style.left = pPos.toString() + "px";}function VAM_SetTopPos(pFld, pPos){if (pFld.style.pixelTop)pFld.style.pixelTop = pPos;else if (pFld.style.posTop)pFld.style.posTop = pPos.toString() + "px";elsepFld.style.top = pPos.toString() + "px";}
function VAM_SetFocus(pFldId){var vFld = VAM_GetById(pFldId);if (vFld.gocDE) 
vFld = vFld.gocDE[0];if ((vFld != null) && (vFld.focus != null) &&((vFld.type == null) || (vFld.type != "hidden")) && 
((this.gVAMFocusFnc == null) || eval(this.gVAMFocusFnc + '(vFld)')) && 
((vFld.disabled == null) || !vFld.disabled) &&
((vFld.style == null) || VAM_IsVisible(vFld))){vFld.focus();if (vFld.select)vFld.select();}} 

function VAM_ParseInt(pVal){var vR = 0;var vNeg = false;for (var vI = 0; vI < pVal.length; vI++){var vC = pVal.charAt(vI);if ((vC >= '0') && (vC <= '9'))vR = (vR * 10) + parseInt(vC);else if (((vC == "-") || (vC == "(")) && (vI==0))vNeg = trueelse if (vC != ")")return NaN;}if (vNeg)vR = -vR;return vR;} 

function VAM_StripTags(pHTML){
return gIsIEMac || (gIsIEWin && !gIsIEWin55) ? pHTML : VAM_RERpl(pHTML, "<(.|\n)+?>", "");} 

function VAM_RERpl(pText, pFind, pReplace){var vRx = new RegExp(pFind, "ig"); return pText.replace(vRx, pReplace);} 

var gVAM_NLTkn = new Array("", "<br />", "\n", "\r", " ");function VAM_NLTkn(pText, pUse, pType){if (pType && pUse) 
{if ((pType == 3) && !gSupportsMultilineTT) 
pType = 4;return VAM_RERpl(pText, "{NEWLINE}", gVAM_NLTkn[pType]);}elsereturn pText;} 

function VAM_AttachEvent(pFld, pEvtName, pCode, pFst){if (typeof(pFld) == "string") 
pFld = VAM_GetById(pFld);var vEv = eval("pFld." + pEvtName + ";");if (typeof(vEv) == "function"){vEv = vEv.toString();vEv = vEv.substring(vEv.indexOf("{") + 1, vEv.lastIndexOf("}"));if (vEv.charAt(vEv.length - 1) != ";")vEv = vEv + ";";}elsevEv = "";if (pFst)vEv = pCode + vEv;elsevEv = vEv + pCode;eval("pFld." + pEvtName + "= function(" + (window.attachEvent == null ? "event){" : "){") + vEv + "}");} 

function VAM_IsVisible(pFld){var vV = true;while (vV && (pFld != null) && (pFld != document.body)){vV = !((pFld.style.visibility == "hidden") || (pFld.style.display == "none"));pFld = pFld.parentNode;} 
return vV;} 

function VAM_InitCond(pCO, pAO){if (pCO.Trim == null)pCO.Trim = true;if (pCO.NotCond == null)pCO.NotCond = false;}
function VAM_InitOneFldCond(pCO, pAO){VAM_InitCond(pCO, pAO);if (pCO.ConvVal == null)pCO.ConvVal = VAM_ConvStrFld;if (pCO.IDToEval != "")VAM_HookupControl(VAM_GetById(pCO.IDToEval), pAO, pCO, false, pCO.HUCtrlFnc);} 

function VAM_InitTwoFldCond(pCO, pAO){VAM_InitOneFldCond(pCO, pAO);if (pCO.ConvVal2 == null)pCO.ConvVal2 = VAM_ConvStrFld;if (pCO.IDToEval2 != "")VAM_HookupControl(VAM_GetById(pCO.IDToEval2), pAO, pCO, false, pCO.HUCtrlFnc2);} 
function VAM_InitMultiCond(pCO, pAO){VAM_InitCond(pCO, pAO);for (var vI = 0; vI < pCO.Conds.length; vI++){var vChild = pCO.Conds[vI];vChild.Action = pAO;if (vChild.InitFnc)vChild.InitFnc(vChild, pAO);} 
} 

function VAM_InitRangeCond(pCO, pAO){VAM_InitOneFldCond(pCO, pAO);if (pCO.MinTxt != null)pCO.Min = pCO.ConvStr(pCO, pCO.MinTxt);elsepCO.Min = null;if (pCO.MaxTxt != null)pCO.Max = pCO.ConvStr(pCO, pCO.MaxTxt);elsepCO.Max = null;} 

function VAM_InitCompValCond(pCO, pAO){VAM_InitOneFldCond(pCO, pAO);pCO.Val = pCO.ConvStr(pCO, pCO.ValTxt);} 

function VAM_EvalMultiCond(pCO){if (pCO.Conds.length == 0)return -1;var vResult = true;var vCanEval = false;if (pCO.ANDOp) 
{
for (var vI = 0; vResult && (vI < pCO.Conds.length); vI++){var vR = VAM_EvalCondition(pCO.Conds[vI]);if (vR == 0)vResult = false; if (vR != -1)vCanEval = true;} 
}else 
{vResult = false;for (var vI = 0; !vResult && (vI < pCO.Conds.length); vI++){var vR = VAM_EvalCondition(pCO.Conds[vI]);if (vR == 1)vResult = true; if (vR != -1)vCanEval = true;} 
}if (!vCanEval)return -1;if (pCO.NOTOp)vResult = !vResult;return vResult ? 1 : 0;} 

function VAM_EvalReqTextCond(pCO){var vResult = true;var vVal = VAM_GetTextValue(pCO.IDToEval, pCO.Trim, pCO.GetText);if (!pCO.Unassnd || (pCO.Unassnd.length == 0))return (vVal != "") ? 1 : 0;if (pCO.CaseIns)vVal = vVal.toUpperCase();for (var vI = 0; vResult && (vI < pCO.Unassnd.length); vI++)if (pCO.Unassnd[vI] == vVal)vResult = false;return vResult ? 1 : 0;} 

function VAM_EvalReqListCond(pCO){var vIdx = VAM_GetSelIdx(pCO.IDToEval, pCO.GetSelIdx);if (vIdx == null)return -1; if (pCO.UnassgnIdx == null) 
pCO.UnassgnIdx = -1;return (vIdx != pCO.UnassgnIdx) ? 1 : 0;} 

function VAM_EvalReqCheckCond(pCO){var vChkdPos = -1;var vDone = false;for (var vI = 0; !((vChkdPos != -1) || vDone); vI++){var vChild = VAM_GetById(pCO.IDToEval + '_' + vI);if (vChild != null){if (vChild.checked)vChkdPos = vI;}else 
vDone = true;}if (!pCO.UnassgnVal) 
pCO.UnassgnVal = -1;return (vChkdPos != pCO.UnassgnVal) ? 1 : 0;} 

function VAM_EvalRangeCond(pCO){var vVal = pCO.ConvVal(pCO, pCO.IDToEval, pCO.GetText);if (vVal == null)return -1;return ( ((pCO.Min == null) || (pCO.Comparer(pCO, pCO.Min, vVal, 5)) ) &&((pCO.Max == null) || (pCO.Comparer(pCO, pCO.Max, vVal, 3)) )) ? 1 : 0;} 

function VAM_EvalComp2FldsCond(pCO){var vVal1 = pCO.ConvVal(pCO, pCO.IDToEval, pCO.GetText);var vVal2 = pCO.ConvVal2(pCO, pCO.IDToEval2, pCO.GetText2);if ((vVal1 == null) || (vVal2 == null))return -1;return pCO.Comparer(pCO, vVal1, vVal2, pCO.Op) ? 1 : 0;} 

function VAM_EvalCompValCond(pCO){var vVal = pCO.ConvVal(pCO, pCO.IDToEval, pCO.GetText);if (vVal == null)return -1;return pCO.Comparer(pCO, vVal, pCO.Val, pCO.Op) ? 1 : 0;} 

function VAM_EvalDTCheckCond(pCO){pCO.Val = null;pCO.Val = pCO.ConvVal(pCO, pCO.IDToEval, pCO.GetText);if (!gVAMCanEval)return -1;return pCO.Val != null ? 1 : 0;} 

function VAM_EvalRegexCond(pCO){if (pCO.Expr == "")return -1;var vVal = VAM_GetTextValue(pCO.IDToEval, pCO.Trim, pCO.GetText);if (vVal == "")return pCO.IBT;var vRx = new RegExp(pCO.Expr, pCO.Flags);return vRx.test(vVal) ? 1 : 0;} 

function VAM_EvalCheckStateCond(pCO){var vFld = VAM_GetById(pCO.IDToEval);if (vFld.checked != null) 
return (vFld.checked == pCO.Chk) ? 1 : 0;elsereturn -1;} 

function VAM_EvalSelIdxCheckCond(pCO){var vFld = VAM_GetById(pCO.IDToEval + "_" + pCO.Idx.toString());if (vFld == null)return -1;return (vFld.checked == pCO.Sel) ? 1 : 0;} 

function VAM_EvalSelIdxListCond(pCO){var vIdx = VAM_GetSelIdx(pCO.IDToEval, pCO.GetSelIdx);if (vIdx == null)return -1;return ((vIdx == pCO.Idx) == pCO.Sel) ? 1 : 0;} 

function VAM_MSCompatCustomCond(pCO){
var vSrc = {clientvalidationfunction:pCO.CVFnc};var vArgs = {Value: "", IsValid:true};eval(pCO.CVFnc + "(vSrc, vArgs);");return vArgs.IsValid ? 1 : 0;} 

function VAM_MSCompatOneFldCustomCond(pCO){
var vSrc = {controltovalidate:pCO.IDToEval, clientvalidationfunction:pCO.CVFnc};var vVal = VAM_GetTextValue(pCO.IDToEval, pCO.Trim, pCO.GetText);if (vVal.length == 0) 
return 1;var vArgs = {Value: vVal, IsValid:true};eval(pCO.CVFnc + "(vSrc, vArgs);");return vArgs.IsValid ? 1 : 0;} 

function VAM_EvalAltCS(pCO){if ((pCO.OCSMode == 0) && gVAMSubmitEvent){pCO.OCSMode = 1; return 1;}else if (pCO.OCSMode == 0)return 0;elsereturn 1;}
function VAM_EvalFixed(pCO){return gVAMSubmitEvent ? 1 : pCO.Res;} 

function VAM_InitValAction(pAO){
if (pAO.SumMsg == null)pAO.SumMsg = "";if (pAO.SelErrMsg == null)pAO.SelErrMsg = VAM_SelErrMsg;if (pAO.SelSumMsg == null)pAO.SelSumMsg = VAM_SelSumMsg;if (pAO.UseFocus == null)pAO.UseFocus = false;if (pAO.ShowAlert == null)pAO.ShowAlert = false;if (pAO.CtlErrCss == null)pAO.CtlErrCss = "";if (pAO.Group == null)pAO.Group = "";if (pAO.EvtToVal == null)pAO.EvtToVal = 0;if (pAO.Blnk == null)pAO.Blnk = false;if (pAO.BlnkCss == null)pAO.BlnkCss = '';if (window.VAM_Blink && (pAO.BlinkFnc == null))pAO.BlinkFnc = VAM_Blink;if (pAO.IsValid == null)pAO.IsValid = true;pAO.BlinkCnt = 0;var vHasHF = pAO.HltFlds != null;if (vHasHF)for (var vI = 0; vI < pAO.HltFlds.length; vI++){var vHF = pAO.HltFlds[vI];vHF.Fld = VAM_GetById(vHF.FID);vHF.OCss = vHF.Fld.className;if (vHF.Fld.HFPassCnt == null)vHF.Fld.HFPassCnt = -1; }
if (pAO.ErrFldID != null){pAO.ErrFld = VAM_GetById(pAO.ErrFldID);if (pAO.ErrFld == null){pAO.Enabled = false;return;}
pAO.OrigCss = pAO.ErrFld.className; pAO.OrigColor = (pAO.ErrFld.style != null) ? pAO.ErrFld.style.color : null;pAO.ImgErrFld = VAM_GetById(pAO.ErrFldID + "_Img");if ((pAO.ErrFld.style != null) && (pAO.ErrFld.style.visibility != "hidden") && pAO.FmttrFnc){if (pAO.TokenRepl) 
VAM_EvalCondition(pAO.Cond); pAO.FmttrFnc(pAO, true);if (window.VAM_StartBlink) 
{gVAMSubmitEvent = true;VAM_StartBlink(pAO);gVAMSubmitEvent = false;}
}}
if (!pAO.IsValid)VAM_SetHiliteFields(pAO, 0);if (!pAO.Ctl && (pAO.VT == "VAL"))gVAM_AONoIDs[gVAM_AONoIDs.length] = pAO;} 

function VAM_DoValidate(pAO, pEvalRes){pAO.IsValid = pEvalRes != 0;if (pAO.FmttrFnc && (pAO.Dspl != 0)){if (pEvalRes != -1){
if (pAO.BeforeFmt)pAO.BeforeFmt(pAO, pEvalRes); pAO.FmttrFnc(pAO, !pAO.IsValid); }
if (pAO.Dspl == 2) 
pAO.ErrFld.style.display = pAO.IsValid ? "none" : "inline";pAO.ErrFld.style.visibility = pAO.IsValid ? "hidden" : "inherit";if (pAO.NEFMd)VAM_NoErrFmt(pAO);if (window.VAM_StartBlink) 
if (!pAO.IsValid)VAM_StartBlink(pAO);elseVAM_StopBlink(pAO);}} 

function VAM_SetHiliteFields(pAO, pEvalRes){if (pAO.HltFlds != null)for (var vI = 0; vI < pAO.HltFlds.length; vI++){var vHF = pAO.HltFlds[vI];if (pEvalRes != 0) 
{if (vHF.Fld.HFPassCnt < gVAM_ValPassCnt)vHF.Fld.className = vHF.OCss;}else 
{if (vHF.Fld.HFPassCnt < gVAM_ValPassCnt){var vCN = (vHF.Txt == 1) ? gVAM_TxtHFCss : gVAM_NonTxtHFCss;if (vCN != "")vHF.Fld.className = vCN;vHF.Fld.HFPassCnt = gVAM_ValPassCnt; }}}} 

function VAM_HUGetChildCtrls(pCO, pAO, pFldID){var vDone = false;for (var vI = 0; !vDone; vI++){vFld = pCO.GetChild(pFldID, vI, 1);if (vFld != null)VAM_HookupControl(vFld, pAO, null, false, null);elsevDone = true;}} 

function VAM_HUGetChild2Ctrls(pCO, pAO, pFldID){var vDone = false;for (var vI = 0; !vDone; vI++){vFld = pCO.GetChild2(pFldID, vI, 1);if (vFld != null)VAM_HookupControl(vFld, pAO, null, false, null);elsevDone = true;}} 

function VAM_GCCheckRadioList(pFldID, pIdx, pMd){var vID = "";if (pMd == 0) 
vID = pIdx == 0 ? pFldID : pFldID + "_" + (pIdx - 1);elsevID = pFldID + "_" + pIdx;return VAM_GetById(vID);} 

var gVAMCanEval = true;function VAM_ConvStrFld(pCO, pID, pGetTextFnc){var vVal = VAM_GetTextValue(pID, pCO.Trim, pGetTextFnc);if (vVal == ""){gVAMCanEval = false;return null;}else{gVAMCanEval = true;return pCO.ConvStr(pCO, vVal);}} 

function VAM_StrConv(pCO, pValue){return pValue;} 

function VAM_CIStrConv(pCO, pValue){return pValue.toUpperCase();} 

function VAM_IntConv(pCO, pValue){var vT = '\\{0}';vT = vT.replace('{0}', pCO.grpsep); pValue = VAM_RERpl(pValue, vT, ''); var vVal = VAM_ParseInt(pValue);vVal = isNaN(vVal) ? null : vVal;if (!pCO.Neg && (vVal != null) && (vVal < 0))vVal = null;if ((vVal != null) && ((vVal > 2147483647) || (vVal < -2147483648)))vVal = null;return vVal;} 

function VAM_DecConv(pCO, pValue){var vT = '\\{0}';vT = vT.replace('{0}', pCO.grpsep); pValue = VAM_RERpl(pValue, vT, ''); var vPtrn = "^\\s*([-\\+])?(\\d+)?(\\{0})?(\\d+)?\\s*$";vPtrn = vPtrn.replace("{0}", pCO.decsep); exp = new RegExp(vPtrn);m = pValue.match(exp);if (m == null)return null;var vPrepVal = (m[1] != null ? m[1] : "")+ (m[2].length>0 ? m[2] : "0")+ "."+ (m[4] != null ? m[4] : "");var vVal = parseFloat(vPrepVal);vVal = isNaN(vVal) ? null : vVal;if (!pCO.Neg && (vVal != null) && (vVal < 0.0))vVal = null;if (pCO.mxdec && (vVal != null)){var vT2 = vVal.toString();var vDP = vT2.indexOf('.');if ((vDP > -1) && (vDP + pCO.mxdec + 1 < vT2.length))vVal = null;}return vVal;} 

function VAM_DateConv(pCO, pValue){pValue = VAM_Trim(pValue);if (pValue.length == 0)return null;var vD = -1;var vM = -1;var vY = -1;var vDateSep = pCO.DateSep;if (pValue.indexOf(vDateSep) == -1)if (pValue.indexOf('-') != -1)vDateSep = '-';elsereturn null;var vDateParts = pValue.split(vDateSep);if (vDateParts.length != 3)return null; var vDateCount = 0;var vPattern = pCO.Pattern.split('|');if (vPattern[0] != "Y"){var vTemp = VAM_ParseInt(vDateParts[0]);if (vTemp >= 1000)vPattern = new Array("Y", "M", "D");}for (var vI = 0; vI < vDateParts.length; vI++){var vPV = VAM_ParseInt(vDateParts[vI]);if (isNaN(vPV)) 
return null;switch (vPattern[vI].charAt(0)){case 'D':if ((vPV <= 0) || (vPV > 31)) 
return null;vD = vPV;break;case 'M':if ((vPV > 12) && (vPV <= 0))return null;vM = vPV;break;case 'Y':if (vPV < 1)return null;if (vPV < 100){if ((pCO.CentBrk == 0) || (vPV <= pCO.CentBrk))vPV = vPV + 2000;elsevPV = vPV + 1900;}vY = vPV;if (vY > 9999)return null;break;} 
} 
vM -= 1;var date = new Date(vY, vM, vD);if (date == null) 
return null;else 
return (vY == date.getFullYear() && vM == date.getMonth() && vD == date.getDate()) ? date.valueOf() : null;} 

function VAM_CurrencyConv(pCO, pValue){
if (pCO.symbol != ""){var vSym = (pCO.symbol == '$') ? "\\$" : pCO.symbol;var vRE = new RegExp("\\s*(" + vSym + ")\\s*", "gi"); var m = pValue.match(vRE);if (m != null) 
{if (m.length > 1)return null;if ((m.length == 1) && (m[0] != "")) 
pValue = pValue.replace(vRE, ""); }}pValue = VAM_Trim(pValue); var vTxtLen = pValue.length;if (vTxtLen == 0)return null;var vPos = 0;var vChar = pValue.charAt(0);var vLParen = vChar == "(";var vNeg = vLParen || (vChar == "-"); if ((vChar == "+") || vNeg){vPos++;vChar = pValue.charAt(vPos);}
var vDecStr = "";var vDone = false;do{if ((vChar >= "0") && (vChar <= "9"))vDecStr += vChar;else if (vChar != pCO.grpsep) 
vDone = true;if (!vDone)if (++vPos < vTxtLen)vChar = pValue.charAt(vPos)elsevDone = true;} while (!vDone);if (vChar == pCO.decsep){vDecStr += "."; var vDigCnt = 0; vDone = false;if (++vPos < vTxtLen){vChar = pValue.charAt(vPos)do{if ((vChar >= "0") && (vChar <= "9")){vDecStr += vChar;vDigCnt++;}elsevDone = true;if (!vDone)if (++vPos < vTxtLen)vChar = pValue.charAt(vPos)elsevDone = true;} while (!vDone);}if (vDigCnt > pCO.decdigits)return null;}
if (vChar == " "){vPos++;if (vPos < vTxtLen)vChar = pValue.charAt(vPos);}var vRParen = vChar == ")";if (vRParen)vPos++;if ((vPos < vTxtLen) || (vLParen != vRParen))return null;var vVal = parseFloat(vDecStr);if (isNaN(vVal))return null;if (vNeg)if (!pCO.Neg) 
vVal = null;elsevVal = -vVal;return vVal;} 

function VAM_Comparer(pCO, pLeftVal, pRightVal, pOp){switch (pOp){case 0: 
return pLeftVal == pRightVal;case 1: 
return pLeftVal != pRightVal;case 2: 
return pLeftVal > pRightVal;case 3: 
return pLeftVal >= pRightVal;case 4: 
return pLeftVal < pRightVal;case 5: 
return pLeftVal <= pRightVal;}return true; } 

function VAM_TextFmttr(pAO, pShow){if (pShow){
if (pAO.TxtErrFld == null)pAO.TxtErrFld = VAM_GetById(pAO.ErrFldID + "_Txt");VAM_SetInnerHTML(pAO.TxtErrFld, VAM_NLTkn(VAM_GetErrMsg(pAO), pAO.Tkns, 1)); }} 

function VAM_TTFmttr(pAO, pShow){if (pShow && pAO.ImgErrFld)pAO.ImgErrFld.title = VAM_NLTkn(VAM_GetErrMsg(pAO), pAO.Tkns, 3); } 

function VAM_AlertFmttr(pAO, pShow){
if (pShow && pAO.ImgErrFld && (pAO.ImgErrFld.onclick == null))pAO.ImgErrFld.onclick = new Function(VAM_GetErrFmtAlert(pAO)); } 

function VAM_HyperLinkFmttr(pAO, pShow){if (pShow){
if (pAO.LnkErrFld == null){pAO.LnkErrFld = VAM_GetById(pAO.ErrFldID + "_Link");pAO.LnkErrFld.href = "javascript: " + VAM_GetErrFmtAlert(pAO); }}} 

function VAM_GetErrFmtAlert(pAO){var vR = "var vAO=gVAMActions[" + pAO.id + "];alert(VAM_NLTkn(VAM_GetErrMsg(vAO), vAO.Tkns, 2));"; if (this.gVAMFcsOnAlt != null){var vId = null;if ((pAO.Ctl != null) && (pAO.Ctl.length > 0)) 
vId = pAO.Ctl[0].id;else if ((pAO.Cond.IDToEval != null) && (pAO.Cond.IDToEval != ""))vId = pAO.Cond.IDToEval;if (vId)vR = vR + "VAM_SetFocus('" + vId + "');";}return vR;}
function VAM_SelErrMsg(pAO){return pAO.ErrMsg;} 

function VAM_SelSumMsg(pAO){return pAO.SumMsg;} 

function VAM_OneFldReplToken(pAO, pText){var vCO = pAO.Cond;if (vCO.IDToEval != "")
return VAM_RERpl(pText, "{TEXTVALUE}", VAM_GetTextValue(vCO.IDToEval, vCO.Trim, vCO.GetText))elsereturn pText;} 

function VAM_TwoFldReplToken(pAO, pText){pText = VAM_OneFldReplToken(pAO, pText);var vCO = pAO.Cond;if (vCO.IDToEval2 != "")
return VAM_RERpl(pText, "{TEXTVALUE2}", VAM_GetTextValue(vCO.IDToEval2, vCO.Trim, vCO.GetText2))elsereturn pText;} 

function VAM_SPReplToken(pText, pCnt, pTName){var vRE = new RegExp("\\{" + pTName + ":([^:}]*):([^:}]*)\\}"); var vMatch = vRE.exec(pText);if ((vMatch != null) && (vMatch.length == 3)){var vOrig = new RegExp("{" + pTName + ":" + vMatch[1] + ":" + vMatch[2] + "}", "gi"); if (pCnt == 1)return pText.replace(vOrig, vMatch[1]);elsereturn pText.replace(vOrig, vMatch[2]);}else 
return pText;} 

function VAM_CanRunActn(pAO){if (pAO.Enabler){var vR = VAM_EvalCondition(pAO.Enabler);return vR != 0; }elsereturn true;} 

function VAM_CanRunVal(pAO){if ((pAO.EvtToVal == 1) && !gVAMSubmitEvent)return false;if ((pAO.EvtToVal == 2) && gVAMSubmitEvent)return false;return VAM_CanRunActn(pAO);} 

var gVAMSubmitEvent = false;function VAM_ValSumInnerHTML(pVSO, pList){var vIH = "";if (pVSO.Hdr != "")vIH = pVSO.Hdr + pVSO.HdrSep;if (pVSO.PreListFnc != null) 
vIH += pVSO.PreListFnc(pVSO) + pList + pVSO.PostListFnc(pVSO);if (pVSO.Ftr != "")vIH += pVSO.Ftr;return vIH;} 

function VAM_ValSumPreDefault(pVSO){return "<p" + VAM_ValSumPreAttributes(pVSO) + ">";} 

function VAM_ValSumPreBullet(pVSO){
return pVSO.BulletTL + "' " + VAM_ValSumPreAttributes(pVSO) + ">";} 

function VAM_ValSumPreSglPara(pVSO){return "<span" + VAM_ValSumPreAttributes(pVSO) + ">";} 

function VAM_ValSumPreDiv(pVSO){return "<div style='width:100%'" + VAM_ValSumPreAttributes(pVSO) + ">";} 

function VAM_ValSumPreAttributes(pVSO){var vAttr = "";if (pVSO.ErrMsgCss != "")vAttr = vAttr + " class='" + pVSO.ErrMsgCss + "'";if (pVSO.LinkTT != "")vAttr = vAttr + " title='" + pVSO.LinkTT + "'";return vAttr;} 

function VAM_ValSumPostDefault(pVSO){return "</p>";} 

function VAM_ValSumPostBullet(pVSO){
if (pVSO.BulletTL.charAt(1) == 'o') 
return "</ol>";elsereturn "</ul>";} 

function VAM_ValSumPostSglPara(pVSO){return "</span>";} 

function VAM_ValSumPostDiv(pVSO){return "</div>";} 

function VAM_ValSumFmtItemList(pVSO, pMsg, pRowNum){var vR = pVSO.ListLdTxt + pMsg + "<br />"; if (pRowNum > 0)vR = pVSO.ListRSep + vR;return vR;} 

function VAM_ValSumFmtBullet(pVSO, pMsg, pRowNum){return "<li>" + pMsg + "</li>";} 

function VAM_ValSumFmtSglPara(pVSO, pMsg, pRowNum){if (pRowNum == 0)return pMsg;elsereturn pVSO.SglParSep + pMsg;} 

var gVAM_CauseVal = true;var gVAM_AltCfmMsg = "";function VAM_DoAction(pAO){pAO.CondResult = -1;if (pAO.Enabled && pAO.ActnFnc){
if (pAO.CanRun && !pAO.CanRun(pAO)){
if (pAO.VT == "VAL")pAO.ActnFnc(pAO, 1);return;}pAO.CondResult = VAM_EvalCondition(pAO.Cond);pAO.ActnFnc(pAO, pAO.CondResult);}} 

function VAM_EvalCondition(pCO){if (pCO.Enabled && pCO.EvalFnc){var vR = pCO.EvalFnc(pCO);if ((vR != -1) && pCO.NotCond)vR = (vR == 1) ? 0 : 1;return vR;}elsereturn -1;} 

function VAM_InitActions(){
if (window.PDP_InitObjects)PDP_InitObjects(false);var vAutoRun = new Array(); for (var vActnID = 0; vActnID < gVAMActions.length; vActnID++){var vAO = gVAMActions[vActnID];vAO.id = vActnID; if (vAO.Cond){var vCO = vAO.Cond;vCO.Action = vAO;if (vCO.InitFnc)vCO.InitFnc(vCO, vAO);}
var vEn = vAO.Enabler;if ((vEn != null) && (vEn.InitFnc != null))vEn.InitFnc(vEn, vAO);if (vAO.ExHU != null)for (var vI = 0; vI < vAO.ExHU.length; vI++)VAM_HookupControl(VAM_GetById(vAO.ExHU[vI]), vAO, null, false, null);if (vAO.InitFnc)vAO.InitFnc(vAO);if (vAO.AutoRun)vAutoRun[vAutoRun.length] = vAO;if (this.VAM_CEMAddAction)VAM_CEMAddAction(vAO);}
for (var vI = 0; vI < vAutoRun.length; vI++)VAM_DoAction(vAutoRun[vI]);if (vAutoRun.length > 0) 
{if (window.VAM_CalcAll) 
VAM_CalcAll();VAM_FixAbsPos(true);}
gVAM_Init = false; } 

function VAM_HookupControl(pFld, pAO, pCO, pAltEvent, pHUCtrlFnc){if (pFld == null)return false;if ((pCO != null) && !pCO.HUEvts) 
return false;if ((pCO == null) || (pHUCtrlFnc == null)){var vFT = false; if (pAO.Ctl == null)pAO.Ctl = new Array;pAO.Ctl[pAO.Ctl.length] = pFld;if (pFld.ActionIDs == null){vFT = true;pFld.ActionIDs = new Array;if (!pAltEvent && !pFld.gocDE) 
{var vUseOnChange = ((pFld.type != null) && ((pFld.type == "text") || (pFld.type == "password") || (pFld.type == "file"))) 
|| (pFld.tagName == "TEXTAREA") || (pFld.tagName == "SELECT");var vEv = vUseOnChange ? pFld.onchange : pFld.onclick;if (typeof(vEv) == "function"){vEv = vEv.toString();vEv = vEv.substring(vEv.indexOf("{") + 1, vEv.lastIndexOf("}"));}elsevEv = "";var vFunc = new Function("VAM_FieldChanged('" + pFld.id + "'); " + vEv);if (vUseOnChange)pFld.onchange = vFunc;elsepFld.onclick = vFunc;}
if (pAO.VT == "VAL")gVAM_ValFlds[gVAM_ValFlds.length] = pFld;}pFld.ActionIDs[pFld.ActionIDs.length] = gVAM_MAId != null ? gVAM_MAId : pAO.id;return vFT;}else{ 
pHUCtrlFnc(pCO, pAO, pFld.id);return false;}} 

function VAM_FindAOById(pClientID){pClientID = pClientID.toUpperCase();for (var vActnID = 0; vActnID < gVAMActions.length; vActnID++){var vAO = gVAMActions[vActnID];if ((vAO.CID != null) && (vAO.CID == pClientID)) 
return vAO;}return null;} 

function VAM_SetEnabled(pAO, pEnabled){if ((pAO != null) && (pEnabled != pAO.Enabled)){pAO.Enabled = pEnabled;if (pEnabled)VAM_DoAction(pAO);else{if (pAO.VT == "VAL")pAO.ActnFnc(pAO, 1);}
if (pAO.CID != null){var vRFM = VAM_GetById(pAO.CID + "_RFM");if (vRFM != null)vRFM.style.visibility = pEnabled ? "inherit" : "hidden";}}} 

function VAM_FieldChanged(pFldId){gVAMSubmitEvent = false;var vFld = VAM_GetById(pFldId);if (!vFld.ActionIDs)return;for (var vI = 0; vI < vFld.ActionIDs.length; vI++)VAM_DoAction(gVAMActions[vFld.ActionIDs[vI]]);VAM_PostValidateFld(vFld);} 

function VAM_ValidateGroup(pGrp, pReal) 
{var vIsValid = true; gVAMSubmitEvent = pReal;if (this.gVAMActions == null) 
return true;for (var vI = 0; vI < gVAMActions.length; vI++){var vAO = gVAMActions[vI];if ((vAO.VT == "VAL") && 
VAM_MatchGroup(pGrp, vAO.Group)) 
{vAO.IsValid = true; VAM_DoAction(vAO); if (!vAO.IsValid)vIsValid = false; }} 
VAM_PostValidate(pGrp, true); return vIsValid;} 

function VAM_MatchGroup(pGRq, pGSp){if (pGRq == "*")return true;pGSp = pGSp.replace("$", "\\$"); var vRx = new RegExp("^(" + pGSp + ")$", "i");return vRx.test(pGRq);} 

function VAM_OnReset(pIsPostBack){if (this.gVAMActions == null) 
return;var vT = (this.gVAM_AlertOnSubmit != null) ? gVAM_AlertOnSubmit : false;gVAM_AlertOnSubmit = false;if (pIsPostBack)VAM_ValidateGroup("*", false); else{
for (var vI = 0; vI < gVAMActions.length; vI++){var vAO = gVAMActions[vI];if (vAO.VT == "VAL") 
{vAO.IsValid = true;if (vAO.Enabled && vAO.ActnFnc)vAO.ActnFnc(vAO, 1); }} 
VAM_PostValidate("*", true); }
gVAM_AlertOnSubmit = vT;if (this.VAM_RunAllFSC) 
VAM_RunAllFSC();} 

function VAM_ValOnSubmit(){
if (!gVAM_CauseVal){gVAM_AltCfmMsg = ""; if (this.VAM_DisableSubmit) 
VAM_DisableSubmit();return true;}gVAM_CauseVal = true;var vGrpFld = VAM_GetById("VAM_Group"); if (vGrpFld == null){var vFlds = document.getElementsByName("VAM_Group");if ((vFlds == null) || (vFlds.length == 0)){gVAM_AltCfmMsg = "";return true;}vGrpFld = vFlds[0];}
var vR = VAM_ValOnSubWGrp(vGrpFld.value);if (!vR && (window.event != null)){window.event.cancelBubble = true;window.event.returnValue = false;}return vR;} 

function VAM_ValOnSubWGrp(pGrp){
for (var vI = 0; vI < 3; vI++){switch (gVAMSubOrder[vI]){case 0: 
if (!VAM_ValidateGroup(pGrp, true))return false; break;case 1: 
if (this.confirm){
if (gVAM_AltCfmMsg != ""){if (!confirm(gVAM_AltCfmMsg))return false;}
else if ((this.gVAMConfMsg != null) &&((this.gVAMConfMsgGrp == "*") || (this.gVAMConfMsgGrp == pGrp)))if (!confirm(gVAMConfMsg))return false;}break;case 2: 
if (this.gVAMCstmSubmitFnc != null)if (!eval(gVAMCstmSubmitFnc + "('" + pGrp + "');"))return false;break;} 
} 
if (this.VAM_DisableSubmit) 
VAM_DisableSubmit();return true;} 

function VAM_ValOnClick(pGrp, pAltMsg){var vGrpFld = VAM_GetById("VAM_Group"); if (vGrpFld == null){var vFlds = document.getElementsByName("VAM_Group");if ((vFlds == null) || (vFlds.length == 0))return;vGrpFld = vFlds[0];}vGrpFld.value = pGrp;gVAM_CauseVal = true; gVAM_AltCfmMsg = pAltMsg ? pAltMsg : ""; } 

function VAM_UpdateOnClick(pFldId, pGrp, pAltMsg){var vFld = VAM_GetById(pFldId);if (vFld == null) 
return;var vFnc = vFld.onclick;if (typeof(vFnc) == "function" ){vFnc = vFnc.toString();vFnc = vFnc.substring(vFnc.indexOf("{") + 1, vFnc.lastIndexOf("}"));}elsevFnc = "";if (pGrp != null){if (pAltMsg == null)pAltMsg = "";vFld.onclick = new Function("VAM_ValOnClick('"+ pGrp+ "','" + pAltMsg + "'); " + vFnc);}else{
if (pAltMsg) 
vFnc = "if (!confirm('" + pAltMsg + "')) return false;" + vFnc;vFld.onclick = new Function("gVAM_CauseVal = false; " + vFnc);}} 

function VAM_InitLinkBtn(pFldId, pGrp, pAltMsg){var vFld = VAM_GetById(pFldId);if (vFld != null) 
{var vOC = "if (this.disabled) return false;"; if (pGrp != null){if (pAltMsg == null)pAltMsg = "";vFld.href = 'javascript: VAM_HrefClick("' + pGrp + '", "' + vFld.href.replace('javascript:', '') + '","' + pAltMsg + '");'; }else 
{
vFld.href = 'javascript:gVAM_CauseVal = false;' + vFld.href.replace('javascript:', '');if (this.gVAMSubmitIDs)vFld.href = vFld.href + ";VAM_DSBody();";if (pAltMsg) 

vOC = vOC + "if (!confirm('" + pAltMsg + "')) return false;";}
VAM_AttachEvent(vFld, "onclick", vOC, false)}} 

function VAM_HrefClick(pGrp, pS, pAltMsg){
if (pS == "")return; var vOK = true;VAM_ValOnClick(pGrp, pAltMsg);if (!VAM_ValOnSubmit())return;if (this.Page_ClientValidate != null) 
if (!Page_ClientValidate())return;eval(pS);} 

function VAM_IsValid(pFld){if (typeof(pFld) == "string") 
pFld = VAM_GetById(pFld);var vA = pFld.ActionIDs;if (vA){for (var vI = 0; vI < vA.length; vI++){var vAO = gVAMActions[vA[vI]];if ((vAO.VT == "VAL") && !vAO.IsValid)return false;} 
return true; }return null;} 

function VAM_Trim(s) {var m = s.match(/^\s*(\S+(\s+\S+)*)\s*$/);return (m == null) ? "" : m[1];} 

function VAM_GetTextValue(pId, pTrim, pGetTextFnc) {if (pGetTextFnc != null)return pTrim ? VAM_Trim(pGetTextFnc(pId)) : pGetTextFnc(pId);var vC = VAM_GetById(pId);if (typeof(vC.value) == "string")return pTrim ? VAM_Trim(vC.value) : vC.value;if (vC.options && vC.selectedIndex)if (vC.selectedIndex == -1)return "";else{var vVal = vC.options[vC.selectedIndex].value;if (pTrim)vVal = VAM_Trim(vVal);return vVal;}
if ((vC.tagName == "TABLE") || (vC.tagName == "SPAN")){var vD = false;for (var vI = 0; !vD; vI++){var vChild = VAM_GetById(pId + '_' + vI);if (vChild != null){if ((vChild.checked != null) && vChild.checked) 
return vChild.value;}else 
vD = true;} 
} 
return ""; } 

function VAM_GetSelIdx(pId, pGetFnc){if (pGetFnc != null)return pGetFnc(pId);var vFld = VAM_GetById(pId);if (vFld.selectedIndex == null) 
return null;return vFld.selectedIndex;} 

function VAM_GetErrMsg(pVO){var vMsg = pVO.SelErrMsg(pVO);if (vMsg == null) 
return "";if (pVO.TokenRepl && (vMsg.indexOf("{") != -1)) 

vMsg = pVO.TokenRepl(pVO, vMsg);return vMsg;} 

function VAM_GetSumMsg(pVO){var vMsg = pVO.SelSumMsg(pVO);if (vMsg == null) 
return "";if (pVO.TokenRepl && (vMsg.indexOf("{") != -1)) 
vMsg = pVO.TokenRepl(pVO, vMsg);if (vMsg == "")vMsg = VAM_GetErrMsg(pVO);return vMsg;} 

var gVAM_ValPassCnt = 0;var gVAM_ValErrMsgs = null;function VAM_PostValidate(pGrp, pAll){if (this.gVAM_PostOnSubmit == null) 
return;if (gVAM_PostOnSubmit){gVAM_ValPassCnt++;gVAM_ValErrMsgs = new Array; var vFstInv = null; for (var vI = 0; vI < gVAM_ValFlds.length; vI++){var vFld = gVAM_ValFlds[vI];if (!VAM_PostValidateBody(vFld, pGrp, true))if (vFstInv == null)vFstInv = vFld;}
for (var vI = 0; vI < gVAM_AONoIDs.length; vI++)
{var vAO = gVAM_AONoIDs[vI];if (!vAO.IsValid)VAM_UpdateValErrMsgs(vAO, true, null);}if (this.gVAM_SSMsgs)for (var vI = 0; vI < gVAM_SSMsgs.length; vI++)gVAM_ValErrMsgs[gVAM_ValErrMsgs.length] = gVAM_SSMsgs[vI];if ((gVAM_ValErrMsgs.length > 0) && pAll){
var vA = gVAM_AlertOnSubmit && ((this.gVAM_SSMsgs == null) || (this.gVAM_SSMsgs.length < gVAM_ValErrMsgs.length));VAM_PostValidateAction(vFstInv, gVAM_FocusOnSubmit, vA);}VAM_UpdateSummaries(pGrp, !pAll);}
if (this.VAM_CEMDoAction)VAM_CEMDoAction();VAM_FixAbsPos(true);} 

function VAM_PostValidateFld(pFld){if (this.gVAM_PostOnChange == null) 
return;if (gVAM_PostOnChange){gVAM_ValPassCnt++;gVAM_ValErrMsgs = new Array; if (!VAM_PostValidateBody(pFld, "*", false))VAM_PostValidateAction(pFld, gVAM_FocusOnChange, gVAM_AlertOnChange);}
var vAO = gVAMActions[pFld.ActionIDs[0]];VAM_AutoUpdateSummaries(vAO.Group);if (this.VAM_CEMDoAction)VAM_CEMDoAction();VAM_FixAbsPos(true);} 

function VAM_PostValidateBody(pFld, pGrp, pS){pFld.IsV = null; if (pFld.ValPassCnt == null) 
pFld.ValPassCnt = 0;if (pFld.ValPassCnt < gVAM_ValPassCnt){var vOFA = new Array; if (!pFld.gocDE && 
(pFld.OFPassCnt != gVAM_ValPassCnt)) 
vOFA[0] = pFld;pFld.ValPassCnt = gVAM_ValPassCnt;pFld.OFPassCnt = gVAM_ValPassCnt; for (var vI = 0; vI < pFld.ActionIDs.length; vI++){vAO = gVAMActions[pFld.ActionIDs[vI]];if (vAO.VT != "VAL") 
continue;if (vAO.ValPassCnt == null)vAO.ValPassCnt = 0;if ((vAO.IsValid != null) && (vAO.CondResult != -1) &&VAM_MatchGroup(pGrp, vAO.Group)){if (!vAO.IsValid){if (vAO.ValPassCnt < gVAM_ValPassCnt) 
VAM_UpdateValErrMsgs(vAO, pS, pFld.id);pFld.IsV = false;}else if (pFld.IsV == null) 
pFld.IsV = true;VAM_GetOtherErrCtl(vOFA, pFld, vAO, pS); } 

else if ((vAO.CondResult == -1) && VAM_MatchGroup(pGrp, vAO.Group)){if (pFld.IsV == null)pFld.IsV = true;VAM_GetOtherErrCtl(vOFA, pFld, vAO, pS); }
vAO.ValPassCnt = gVAM_ValPassCnt;VAM_SetHiliteFields(vAO, vAO.CondResult);} 
if ((pFld.IsV != null) && (this.gVAM_ErrCtlCss != null)){
for (var vI = 0; vI < vOFA.length; vI++)VAM_PostValidateErrCtl(vOFA[vI], vOFA[vI].IsV);} 
} 
return pFld.IsV != false; } 

function VAM_GetOtherErrCtl(pOFA, pFld, pAO, pS){
if (!pFld.gocDE && (pS || (pAO.EvtToVal == 1)))return;var vMS = pFld.gocDE != null; var vFA = vMS ? pFld.gocDE : pAO.Ctl;for (var vJ = 0; vJ < vFA.length; vJ++){var vO = vFA[vJ];if (vO != pFld){
var vUPC = (vO.OFPassCnt == null) || (vO.OFPassCnt < gVAM_ValPassCnt); if (vUPC) 
pOFA[pOFA.length] = vO;if (vUPC || !pAO.IsValid)vO.IsV = pAO.IsValid;if (VAM_IsValid(vO) == false)vO.IsV = false;vO.OFPassCnt = gVAM_ValPassCnt;} 
} if (pFld.gocfld) 
{if (VAM_IsValid(pFld.gocfld) == false)pFld.IsV = false;}} 

function VAM_UpdateValErrMsgs(pAO, pS, pFldId){
if (!pS || (pAO.EvtToVal != 2)){var vMsg = pS ? VAM_GetSumMsg(pAO) : VAM_GetErrMsg(pAO);if (vMsg != ""){
var vId = pAO.Ctl ? pAO.Ctl[0].id : pFldId; 
gVAM_ValErrMsgs[gVAM_ValErrMsgs.length] = {Msg: vMsg, Grp: pAO.Group, FldId: vId}; }}} 

function VAM_PostValidateErrCtl(pF, pIsV){
if (pF.SetErrStyle){pF.SetErrStyle(pF.id, pIsV);return;}
var vCss = "";var vCB = false;if ((pF.tagName == "INPUT") && ((pF.type == "checkbox") || (pF.type == "radio"))){vCss = gVAM_ErrChkCss;vCB = true;}else if (pF.tagName == "SELECT")vCss = gVAM_ErrLstCss;if (vCss == "")vCss = gVAM_ErrCtlCss;if ((vCss != "") && (pF.className != null)){
if (!vCB || (gVAM_CBECCMd < 2))if (pIsV){if (pF.OrigCss != null)pF.className = pF.OrigCss;}else{if (pF.OrigCss == null) 
{
if (pF.style.OrigCss != null){pF.OrigCss = pF.style.OrigCss;if (pF.OrigCss == "")pF.OrigCss = "";}else
pF.OrigCss = pF.className;}if (vCss != "")pF.className = vCss;}
if (vCB && (gVAM_CBECCMd != 1)) 
{vL = pF.nextSibling;if (vL && (vL.tagName == "LABEL"))vL.className = pIsV ? "" : vCss;}} 
} 

function VAM_PostValidateAction(pFld, pFocus, pAlert){if (pAlert){
var vMsg = gVAM_AlertTemplate;if ((vMsg == "") || (vMsg.indexOf("{0}") > -1)){var vBody = "";for (var vI = 0; vI < gVAM_ValErrMsgs.length; vI++){var vCnt = vI + 1;var vLine = "";if (gVAM_AlertListStyle)vLine = gVAM_AlertLeadText.replace("#", vCnt) + gVAM_ValErrMsgs[vI].Msg + "\n";elsevLine = gVAM_ValErrMsgs[vI].Msg + " ";vBody = vBody + vLine;} 
if (vMsg == "")vMsg = vBody;elsevMsg = vMsg.replace("{0}", vBody);}alert(VAM_StripTags(VAM_NLTkn(vMsg, true, 2))); }
if (pFocus && (pFld != null)) 
{var vCode = "javascript:VAM_SetFocus('" + pFld.id + "');"; setTimeout(vCode, 10);}} 

function VAM_UpdateSummaries(pGrp, pAutoUpd){if (this.gVAM_ValSummary != null){for (var vI = 0; vI < this.gVAM_ValSummary.length; vI++){var vVSO = this.gVAM_ValSummary[vI];if (!pAutoUpd || (vVSO.Showing && vVSO.AutoUpd))VAM_UpdateValSum(vVSO, pGrp);}}
if (this.gVAM_ValUpdFnc)eval(this.gVAM_ValUpdFnc);} 

function VAM_AutoUpdateSummaries(pGrp){if (this.gVAM_ValSummary != null){for (var vI = 0; vI < this.gVAM_ValSummary.length; vI++){var vVSO = this.gVAM_ValSummary[vI];if (vVSO.Showing && vVSO.AutoUpd){VAM_PostValidate(pGrp, false);return;}}}} 

function VAM_UpdateValSum(pVSO, pGrp){if (pVSO.fAllGrps == null) 
{pVSO.fAllGrps = pVSO.Grp == "*";if (!pVSO.fAllGrps){pVSO.fGroups = pVSO.Grp.split('|');for (var vI = 0; vI < pVSO.fGroups.length; vI++)pVSO.fGroups[vI] = VAM_Trim(pVSO.fGroups[vI]);}}var vList = "";if (pVSO.fAllGrps || VAM_MatchGroup(pGrp, pVSO.Grp)){
var vPosNum = 0;for (var vI = 0; vI < gVAM_ValErrMsgs.length; vI++){
var vMsgGrp = gVAM_ValErrMsgs[vI].Grp;var vMatch = pVSO.fAllGrps; if (!vMatch)for (var vJ = 0; !vMatch && (vJ < pVSO.fGroups.length); vJ++)vMatch = VAM_MatchGroup(pVSO.fGroups[vJ], vMsgGrp);if (vMatch){if (pVSO.FmtListFnc != null){vMsg = gVAM_ValErrMsgs[vI].Msg;if (pVSO.Links && (gVAM_ValErrMsgs[vI].FldId != null)) 
vMsg = "<a href=\"javascript:VAM_SetFocus('" + gVAM_ValErrMsgs[vI].FldId + "');\">"+ vMsg+"</a>";vList = vList + pVSO.FmtListFnc(pVSO, vMsg, vPosNum);vPosNum++;}else 
vList = "!"; } 
} 
} 
else 
return;pVSO.Showing = (vList != "");vFld = VAM_GetById(pVSO.ValSumID);if (pVSO.Showing){VAM_SetInnerHTML(vFld, VAM_NLTkn(pVSO.GetInner(pVSO, vList), true, 1)); vFld.style.visibility = "inherit";if (!pVSO.InvSpc)vFld.style.display = "block";if (pVSO.RelCtl != null){var vRC = VAM_GetById(pVSO.RelCtl);vRC.style.visibility = "inherit";if (!pVSO.RCIS)vRC.style.display = "inline"; }
if (gVAMSubmitEvent && (pVSO.Scroll != null) && vFld.scrollIntoView)vFld.scrollIntoView(pVSO.Scroll == 1);}else{if (!pVSO.InvSpc)vFld.style.display = "none";vFld.style.visibility = "hidden";VAM_SetInnerHTML(vFld, "");if (pVSO.RelCtl != null){var vRC = VAM_GetById(pVSO.RelCtl);vRC.style.visibility = "hidden";if (!pVSO.RCIS)vRC.style.display = "none";}}} 

function VAM_FixAbsPos(pVal){
if (pVal && this.gVAMValUpdFnc)eval(this.gVAMValUpdFnc);if (this.VAM_MoveSpinners) 
VAM_MoveSpinners();if (this.TMTB_Resize) 
TMTB_Resize();}