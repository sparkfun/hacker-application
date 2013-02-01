// Copyright 2003 - 2006 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.4


var gVAM_DTTBIds = null;function VAM_InitSpinner(pTBId, pObj){var vTBFld = VAM_GetById(pTBId);vTBFld.Spin = pObj;var vBtnFld = VAM_GetById(pTBId + "_Btns");if (vBtnFld != null){
if (gVAM_DTTBIds == null){gVAM_DTTBIds = new Array();}VAM_MoveSpinner(pTBId); gVAM_DTTBIds[gVAM_DTTBIds.length] = pTBId;}} 

function VAM_MoveSpinners(){if (gVAM_DTTBIds != null)for (var vI = 0; vI < gVAM_DTTBIds.length; vI++)VAM_MoveSpinner(gVAM_DTTBIds[vI]);} 

function VAM_MoveSpinner(pTBId){var vBtnFld = VAM_GetById(pTBId + "_Btns");if (vBtnFld != null){var vTBFld = VAM_GetById(pTBId);var vBP = vBtnFld.parentNode;if (vBP && vBP.style && (vBP.style.verticalAlign != "middle") && (vBtnFld.style.verticalAlign != "middle")){
var vY = (vTBFld.offsetHeight - vBtnFld.offsetHeight) / 2;if (vY > 0)vBtnFld.style.marginTop = vY.toString() + "px";}var vVis = VAM_IsVisible(vTBFld);vBtnFld.style.visibility = vVis ? "inherit" : "hidden";}} 

var gVAM_SpinTracking = false;var gVAM_SpinSpeed = 500;var gVAM_SpinIncCnt = 0;var gVAM_SpinTimerID = 0;function VAM_SpinClickDown(pTBId, pIncBy, pImgId, pImgUrl){if (!VAM_CanEdit(pTBId)) return;VAM_SpinClear();var vTBFld = VAM_GetById(pTBId);var vSO = vTBFld.Spin;gVAM_SpinSpeed = vSO.Spd1; gVAM_SpinIncCnt = 0;if (pImgUrl != ''){vImgFld = VAM_GetById(pImgId);if (vImgFld && vImgFld.src)vImgFld.src = pImgUrl;}VAM_SpinInc(pTBId, pIncBy);var vCode = "javascript:VAM_SpinRepeat('" + pTBId + "'," + pIncBy + ")";gVAM_SpinTimerID = setTimeout(vCode, gVAM_SpinSpeed);gVAM_SpinTracking = true;} 

function VAM_SpinRepeat(pTBId, pIncBy){if (gVAM_SpinTracking){VAM_SpinInc(pTBId, pIncBy);var vCode = "javascript:VAM_SpinRepeat('" + pTBId + "'," + pIncBy + ")";gVAM_SpinTimerID = setTimeout(vCode, gVAM_SpinSpeed);gVAM_SpinIncCnt++;if (gVAM_SpinIncCnt == 4) 
{var vTBFld = VAM_GetById(pTBId);var vSO = vTBFld.Spin;gVAM_SpinSpeed = vSO.Spd2;}}} 

function VAM_SpinClickUp(pTBId, pImgId, pImgUrl){
if (gVAM_SpinTracking){var vF = VAM_GetById(pTBId);if (vF.focus){vF.focus();if (vF.select)vF.select();}}VAM_SpinClear();if (pImgUrl != ''){vImgFld = VAM_GetById(pImgId);if (vImgFld && vImgFld.src)vImgFld.src = pImgUrl;}} 

function VAM_SpinClear(){gVAM_SpinTracking = false;if (gVAM_SpinTimerID != 0)clearTimeout(gVAM_SpinTimerID);gVAM_SpinTimerID = 0;}
function VAM_SpinInc(pTBId, pIncBy){if (!VAM_CanEdit(pTBId)) return; var vTBFld = VAM_GetById(pTBId);var vSO = vTBFld.Spin;var vVal = VAM_GetDTTBValue(pTBId);if ((vVal == null) && VAM_TBIsBlank(vTBFld)){var vN = 0;if ((vSO.Max != null) && (pIncBy < 0))vN = vSO.Max;else if ((vSO.Min != null) && (pIncBy > 0))vN = vSO.Min;VAM_SetDTTBValue(pTBId, vN);return;}if (vVal != null) 
{vVal = vVal + pIncBy;if (vSO.Digs == null) 
{vSO.Round = false;var vAO = VAM_FindAOById(pTBId);if (vAO){var vDigs = (vAO.Cond.mxdec != null) ? vAO.Cond.mxdec : vAO.Cond.decdigits; if (vDigs != null){vSO.Round = true;if ((vDigs < 0) || (vDigs > 5)) 
vDigs = 5;vSO.Digs = vDigs;}elsevSO.Digs = 0; }}if (vSO.Round)if (vSO.Digs == 0) 
vVal = Math.round(vVal);else{var vSF = Math.pow(10.0, vSO.Digs); var vSV = vVal * vSF; vSV = Math.round(vSV); vVal = vSV / vSF; }
if ((vSO.Max != null) && (pIncBy > 0) && (vSO.Max < vVal))vVal = vSO.Max;else if ((vSO.Min != null) && (pIncBy < 0) && (vSO.Min > vVal))vVal = vSO.Min;VAM_SetDTTBValue(pTBId, vVal);}} 

function VAM_RelocatePopup(pPF){
var vPN = pPF.parentNode;if ((vPN != null) && (vPN != document.body) && (vPN.tagName != "FORM") && (window.__smartNav == null)){if ((document.body.removeChild != null) &&((document.body.appendChild != null) || (document.body.outerHTML != null))){var vM = vPN.removeChild(pPF);if (!gIsOpera7 && (vM.outerHTML != null)) 
{document.write(vM.outerHTML);}elsedocument.body.appendChild(vM);}}} 
function VAM_GetOffsetX(pFld, pIP, pDir){var vLP = pIP;if (pFld.offsetParent)for (var vPar = pFld.offsetParent;(vPar != null);vPar = vPar.offsetParent){vLP = vLP + pDir * vPar.offsetLeft;if (gIsSafari && (vPar.style.position == "absolute"))break;} 
elsefor (var vPar = pFld.parentNode;(vPar != document.body) && (vPar != null);vPar = vPar.parentNode){if (vPar.style.position == "absolute"){vLP = vLP + pDir * vPar.offsetLeft;break;}} 
return vLP;} 
function VAM_GetOffsetY(pFld, pIP, pDir){var vTP = pIP;if (pFld.offsetParent)for (var vPar = pFld.offsetParent;(vPar != null);vPar = vPar.offsetParent){vTP = vTP + pDir * vPar.offsetTop;if (gIsSafari && (vPar.style.position == "absolute"))break;} 
elsefor (var vPar = pFld.parentNode;(vPar != document.body) && (vPar != null);vPar = vPar.parentNode){if (vPar.style.position == "absolute"){vTP = vTP + pDir * vPar.offsetTop;break;}} 
return vTP;} 
function VAM_SetLeftPos(pFld, pLeftPos){if (pFld.style.pixelLeft)pFld.style.pixelLeft = pLeftPos;else if (pFld.style.posLeft)pFld.style.posLeft = pLeftPos.toString() + "px";elsepFld.style.left = pLeftPos.toString() + "px";}function VAM_SetTopPos(pFld, pTopPos){if (pFld.style.pixelTop)pFld.style.pixelTop = pTopPos;else if (pFld.style.posTop)pFld.style.posTop = pTopPos.toString() + "px";elsepFld.style.top = pTopPos.toString() + "px";}
function VAM_CanEdit(pFId){var vFld = VAM_GetById(pFId);return !(vFld.disabled || vFld.readOnly);} 
