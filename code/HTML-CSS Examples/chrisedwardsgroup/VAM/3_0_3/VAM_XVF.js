// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


function VAM_EvalVisCond(pCO){
return VAM_IsVisible(VAM_GetById(pCO.IDToEval)) == pCO.Vis ? 1 : 0;} 

function VAM_EvalEnabledCond(pCO){var vFld = VAM_GetById(pCO.IDToEval);if (vFld.disabled == null)return -1;return (vFld.disabled != pCO.IsEnab) ? 1 : 0;} 

function VAM_EvalReadOnlyCond(pCO){var vFld = VAM_GetById(pCO.IDToEval);if (vFld.readOnly == null)return -1;return (vFld.readOnly == pCO.IsRO) ? 1 : 0;} 

function VAM_EvalClassNameCond(pCO){var vFld = VAM_GetById(pCO.IDToEval);if (vFld.className == null)return -1;return (vFld.className.toUpperCase() == pCO.Css) ? 1 : 0; } 
function VAM_EvalCompToValAttrCond(pCO){var vFldVal = VAM_GetAttrCondVal(pCO);if (vFldVal == null)return -1;return VAM_Comparer(pCO, vFldVal, pCO.Val, pCO.Op) ? 1 : 0;} 

function VAM_EvalBTxtLenCond(pCO){var vVal = VAM_GetTextValue(pCO.IDToEval, pCO.Trim, pCO.GetText);if (pCO.IBT && (vVal.length == 0))return 1;pCO.Count = pCO.CntElFnc(pCO, vVal);if (pCO.IDToEval2 != ""){vVal = VAM_GetTextValue(pCO.IDToEval2, pCO.Trim, pCO.GetText);pCO.Count = pCO.Count + pCO.CntElFnc(pCO, vVal); }if (pCO.Min != 0)if (pCO.Min > pCO.Count){pCO.Diff = pCO.Min - pCO.Count; return 0;}if (pCO.Max != 0)if (pCO.Max < pCO.Count){pCO.Diff = pCO.Count - pCO.Max;return 0;}return 1;} 

function VAM_TxtLenReplToken(pAO, pText){pText = VAM_OneFldReplToken(pAO, pText);pText = VAM_SPReplToken(pText, pAO.Cond.Count, "COUNT");pText = VAM_SPReplToken(pText, pAO.Cond.Diff, "EXCEEDS");pText = VAM_RERpl(pText, "{COUNT}", pAO.Cond.Count);return VAM_RERpl(pText, "{EXCEEDS}", pAO.Cond.Diff);} 

function VAM_CntChars(pCO, pText){return pText.length;} 

function VAM_NoErrFmt(pAO){if (!pAO.NEFInit){pAO.NEFInit = true;pAO.NEFFld = VAM_GetById(pAO.ErrFldID + "_NEF"); }if (!pAO.NEFFld) 
return;var vVis = false; if ((pAO.NEFMd == 1) 
|| (pAO.NEFMd == 3)) 
vVis = pAO.IsValid;else if (pAO.NEFMd == 2) 
{ 
vVis = false;if (pAO.LastCR != null) 
vVis = (pAO.CondResult == 1) &&((pAO.LastCR == 0) || ((pAO.LastCR == 1) && (pAO.LastCR2 == 0))); pAO.LastCR2 = pAO.LastCR; pAO.LastCR = pAO.CondResult;}else 
vVis = false;pAO.NEFFld.style.display = vVis ? "inline" : "none";pAO.NEFFld.style.visibility = vVis ? "inherit" : "hidden";} 

function VAM_ValSumPreTbl(pVSO){return "<table width='100%' border='0' cellspacing='0' " + VAM_ValSumPreAttributes(pVSO) + ">";} 

function VAM_ValSumPostTbl(pVSO){return "</table>";} 

function VAM_ValSumFmtTbl(pVSO, pMsg, pRowNum){var vCss = (pRowNum % 2 == 0) ? pVSO.EvenRowCss : pVSO.OddRowCss;return "<tr><td class='" + vCss + "'>" + pMsg + "</td></tr>";} 

function VAM_GetAttrCondVal(pCO){var vFld = VAM_GetById(pCO.IDToEval);var vFldVal = null;if (pCO.AT == "A")eval("vFldVal = vFld." + pCO.Name + ";");elseeval("vFldVal = vFld.style." + pCO.Name + ";");if (pCO.DT != "string")vFldVal = eval(vFldVal);return vFldVal;} 

function VAM_DisableSubmit(){if (this.gVAMSubmitIDs != null){
var vCode = "javascript:VAM_DSBody();";setTimeout(vCode, 20);}return true;} 
function VAM_DSBody(){for (var vI = 0; vI < gVAMSubmitIDs.length; vI++){var vFld = VAM_GetById(gVAMSubmitIDs[vI]);if (vFld.disabled != null)vFld.disabled = true;}} 

function VAM_InitMenuControl(pFldID, pGrp, pAll, pLoc){var vFld = VAM_GetById(pFldID);if (vFld != null){VAM_IMCUpdate(vFld, pGrp, pAll, pLoc);VAM_IMCChildren(vFld, pGrp, pAll, pLoc);}}
function VAM_IMCChildren(pFld, pGrp, pAll, pLoc){var vChildren = pFld.childNodes != null ? pFld.childNodes : pFld.children;if (vChildren != null)for (var vI = 0; vI < vChildren.length; vI++){if (vChildren[vI] != null){VAM_IMCUpdate(vChildren[vI], pGrp, pAll, pLoc);VAM_IMCChildren(vChildren[vI], pGrp, pAll, pLoc); }}}
function VAM_IMCUpdate(pFld, pGrp, pAll, pLoc){if (pFld.onclick != null){var vEv = (pLoc == 0) ? pFld.onclick :((pLoc == 1) ? pFld.onmouseup : pFld.onmousedown);if (typeof(vEv) == "function"){vEv = vEv.toString();vEv = vEv.substring(vEv.indexOf("{") + 1, vEv.lastIndexOf("}"));var vPos = vEv.indexOf("__doPostBack");if (vPos > -1)vEv = vEv.replace("__doPostBack", "if (VAM_ValOnSubWGrp('" + pGrp + "')) __doPostBack");else if (!pAll)return false;elsevEv = "if (VAM_ValOnSubWGrp('" + pGrp + "')) { " + vEv + " }";var vFnc = new Function(vEv);if (pLoc == 0)pFld.onclick = vFnc;else if (pLoc == 1)pFld.onmouseup = vFnc;elsepFld.onmousedown = vFnc;return true;}}return false;}