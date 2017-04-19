// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


var gVAMCalcCnt = 0;function VAM_CalcInit(pAO){VAM_CalcInitList(pAO, pAO.Exp);if (pAO.SVID)pAO.SVF = VAM_GetById(pAO.SVID);if (pAO.Tkn) 
{
var vTID = pAO.CID2 + "_Token";var vF = VAM_GetById(vTID);if (vF == null){var vT = pAO.SVF.innerHTML;vT = vT.replace(new RegExp(pAO.Tkn, "gi"), "<span id='" + vTID + "'></span>");VAM_SetInnerHTML(pAO.SVF, vT);pAO.SVF = VAM_GetById(vTID); }}} 
function VAM_CalcInitList(pAO, pExp){var vT = 0; for (var vI = 0; vI < pExp.length; vI++){var vIC = pExp[vI]; if (vIC.Cond != null)vIC.Cond.InitFnc(vIC.Cond, pAO);if (vIC.Exp != null) 
VAM_CalcInitList(pAO, vIC.Exp); if (vIC.ExpT != null) 
VAM_CalcInitList(pAO, vIC.ExpT); if (vIC.ExpF != null) 
VAM_CalcInitList(pAO, vIC.ExpF); } 
pAO.CalcVal = 0.0; pAO.Cnt = -1; } 

function VAM_CalcAction(pAO, pEvalRes){gVAMCalcCnt++;var vVal = VAM_Calculate(pAO);VAM_CalcShowValue(pAO, vVal);if (!gVAMSubmitEvent && pAO.Val)VAM_FieldChanged(pAO.CID2); } 

function VAM_Calculate(pAO){if (pAO.Cnt != gVAMCalcCnt){pAO.Cnt = gVAMCalcCnt; var vVal = VAM_CalcExp(pAO, pAO.Exp);if (!isNaN(vVal)) 
vVal = VAM_Round(vVal, pAO.RM, pAO.DP);pAO.CalcVal = vVal; return vVal;}elsereturn pAO.CalcVal;} 

function VAM_CalcExp(pAO, pExp){
if (!pExp)return 0;var vT = 0; for (var vI = 0; vI < pExp.length; vI++){var vIC = pExp[vI]; var vVal = 0;if (vIC.Exp != null) 
vVal = VAM_CalcExp(pAO, vIC.Exp);else if (vIC.Const != null) 
vVal = vIC.Const;else if (vIC.LID != null) 
{var vFd = false; if (vIC.CFSI != null) 
{for (vJ = 0; !vFd && (vJ < vIC.CFSI.length); vJ++){var vCFSI = vIC.CFSI[vJ];var vSI = VAM_GetById(vIC.LID).selectedIndex;if ((vCFSI.SI <= vSI) && (vCFSI.EI >= vSI)){vFd = true;if (vCFSI.Const == null) 
vVal = NaN;elsevVal = vCFSI.Const;}} 
}if (!vFd)if (vIC.ConstNM == null) 
vVal = NaN;elsevVal = vIC.ConstNM;}
else if (vIC.CID != null) 
{vVal = VAM_GetDTTBValue(vIC.CID);if (vVal == null) 
{if (VAM_Trim(VAM_GetById(vIC.CID).value) == "") 
vVal = vIC.BlkZ ? 0.0 : NaN;else 
vVal = vIC.InvZ ? 0.0 : NaN;}}else if (vIC.CalcID != null) 
{vVal = VAM_CalcOne(vIC.CalcID);if (isNaN(vVal) && vIC.InvZ)vVal = 0.0;}else if (vIC.Cond != null) 
{var vR = VAM_EvalCondition(vIC.Cond);switch (vR){case 1:vVal = VAM_CalcExp(pAO, vIC.ExpT);break;case 0:vVal = vIC.InvWF ? NaN : VAM_CalcExp(pAO, vIC.ExpF);break;case -1:switch (vIC.CEMd){case 0: 
vVal = NaN;break;case 1: 
vVal = 0.0;break;case 2: 
vVal = VAM_CalcExp(pAO, vIC.ExpT);break;case 3: 
vVal = VAM_CalcExp(pAO, vIC.ExpF);break;} 
break;} 
}
if (vIC.CCalc != null)vVal = vIC.CCalc(pAO.CID2, vIC, vVal);if (isNaN(vVal))return NaN;switch (vIC.Op){case 0: 
vT = vT + vVal;break;case 1: 
vT = vT - vVal;break;case 2: 
vT = vT * vVal;break;case 3: 
if (vVal != 0.0)vT = vT / vVal;elsereturn NaN;break;}} 
return vT} 

function VAM_CalcShowValue(pAO, pVal){var vF = pAO.SVF; if (vF){if (pAO.OrigCss == null) 
{
pAO.OrigCss = (vF.className != pAO.InvCss) ? vF.className : "";}else if (vF.className == pAO.InvCss)vF.className = pAO.OrigCss != " " ? pAO.OrigCss : "";if (!isNaN(pVal)) 
{if (pAO.SVT == 0) 
{VAM_SetInnerHTML(vF, pAO.ToStrFnc(pAO, pVal));}else 
{if (pAO.SVT == 1) 
{
if (Math.round(pVal) != pVal)
pVal = VAM_Round(pVal, 0, 0);}VAM_SetDTTBValue(vF.id, pVal);}}else{vF.className = pAO.InvCss;var vIL = pAO.InvLbl;if (pAO.SVT != 0) 
{if (vIL == "") 
{var vTBAO = VAM_FindAOById(vF.id);if (vTBAO.DefVal != null)vIL = vTBAO.DefVal;}vF.value = vIL;}elseVAM_SetInnerHTML(vF, vIL);}}} 

function VAM_Round(pVal, pMd, pDP){if (pDP == -1)return pVal;var vSF = Math.pow(10.0, pDP); var vSV = pVal * vSF; switch (pMd){case 0: 
vSV = Math.floor(Math.abs(vSV));if (pVal < 0)vSV = -vSV;return vSV / vSF; case 1: 
var vNV = Math.floor(vSV); if ((vSV != vNV) && (vNV % 2 == 1)) 
{ 
vNV = Math.round(vSV);}return vNV / vSF; case 2: 

vSV = Math.round(Math.abs(vSV)); if (pVal < 0)vSV = -vSV;return vSV / vSF; case 3:vSV = Math.ceil(vSV);return vSV / vSF; case 4:vSV = Math.ceil(Math.abs(vSV));if (pVal < 0)vSV = -vSV;return vSV / vSF; } 
return 0; } 

function VAM_CalcAll(){for (var vAID = 0; vAID < gVAMActions.length; vAID++){vAO = gVAMActions[vAID];if (vAO.VT == "CALC")VAM_DoAction(vAO);}} 

function VAM_CalcOne(pID){var vAO = VAM_FindAOById(pID);if (vAO != null)return VAM_Calculate(vAO);elsereturn NaN;} 

function VAM_CalcFromCond(pID){var vR = VAM_CalcOne(pID);return isNaN(vR) ? "" : vR.toString();} 
