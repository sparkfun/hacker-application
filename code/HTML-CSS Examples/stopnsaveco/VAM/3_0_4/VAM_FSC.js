// Copyright 2003 - 2006 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.4


var gVAM_InFC = false;function VAM_InitFieldState(pAO){pAO.ChgFld = VAM_GetById(pAO.IDToChg);if (pAO.CT_Vis == null)pAO.CT_Vis = true;if (pAO.CF_Vis == null)pAO.CF_Vis = true;if (pAO.CT_Enab == null)pAO.CT_Enab = true;if (pAO.CF_Enab == null)pAO.CF_Enab = true;if (pAO.CT_RO == null)pAO.CT_RO = false;if (pAO.CF_RO == null)pAO.CF_RO = false;if (pAO.CT_Chk == null)pAO.CT_Chk = true;if (pAO.CF_Chk == null)pAO.CF_Chk = true;var vDefVal = "";var vDefHtml = "";var vDefURL = "";var vDefCss = "";if (pAO.ChgFld != null){if (pAO.CT_Css != pAO.CF_Css)if (pAO.ChgFld.className != null)vDefCss = pAO.ChgFld.className;if (pAO.CT_Val != pAO.CF_Val) 
if (pAO.ChgFld.value != null)vDefVal = pAO.ChgFld.value;if (pAO.CT_Html != pAO.CF_Html)if (pAO.ChgFld.innerHTML != null)vDefHtml = pAO.ChgFld.innerHTML;if (pAO.CT_URL != pAO.CF_URL)if (pAO.ChgFld.href != null)vDefURL = pAO.ChgFld.href;else if (pAO.ChgFld.src != null)vDefURL = pAO.ChgFld.src;if ((pAO.CT_OName != null) && (pAO.CF_OName == null)) 
{pAO.CF_OName = pAO.CT_OName;eval("pAO.CF_OVal=pAO.ChgFld." + pAO.CT_OName + ";");if (typeof(pAO.CF_OVal) == "string")pAO.CF_OVal = '"' + pAO.CF_OVal + '"';}if ((pAO.CF_OName != null) && (pAO.CT_OName == null)) 
{pAO.CT_OName = pAO.CF_OName;eval("pAO.CT_OVal=pAO.ChgFld." + pAO.CF_OName + ";");if (typeof(pAO.CT_OVal) == "string")pAO.CT_OVal = '"' + pAO.CT_OVal + '"';}if ((pAO.CT_O2Name != null) && (pAO.CF_O2Name == null)) 
{pAO.CF_O2Name = pAO.CT_O2Name;eval("pAO.CF_O2Val=pAO.ChgFld." + pAO.CT_O2Name + ";");if (typeof(pAO.CF_O2Val) == "string")pAO.CF_O2Val = '"' + pAO.CF_O2Val + '"';}if ((pAO.CF_O2Name != null) && (pAO.CT_O2Name == null)) 
{pAO.CT_O2Name = pAO.CF_O2Name;eval("pAO.CT_O2Val=pAO.ChgFld." + pAO.CF_O2Name + ";");if (typeof(pAO.CT_O2Val) == "string")pAO.CT_O2Val = '"' + pAO.CT_O2Val + '"';}}if (pAO.CT_Css == null)pAO.CT_Css = vDefCss;if (pAO.CF_Css == null)pAO.CF_Css = vDefCss;if (pAO.CT_Val == null)pAO.CT_Val = vDefVal;if (pAO.CF_Val == null)pAO.CF_Val = vDefVal;if (pAO.CT_Html == null)pAO.CT_Html = vDefHtml;if (pAO.CF_Html == null)pAO.CF_Html = vDefHtml;if (pAO.CT_URL == null)pAO.CT_URL = vDefURL;if (pAO.CF_URL == null)pAO.CF_URL = vDefURL;if (pAO.InvPS == null)pAO.InvPS = true;if (pAO.ChgFld != null){if (pAO.ChgFld.style != null){pAO.ODspl = pAO.ChgFld.style.display;if (!pAO.InvPS && (pAO.ChgFld.style.visibility == "hidden"))pAO.ChgFld.style.display = "none";}}elsepAO.Enabled = false;} 

function VAM_InitMultiAction(pAO){gVAM_MAId = pAO.id; for (var vI = 0; vI < pAO.Actions.length; vI++){var vActn = pAO.Actions[vI];vActn.Cond = pAO.Cond; if (vActn.ExHU != null)for (var vJ = 0; vJ < vActn.ExHU.length; vJ++) 
VAM_HookupControl(VAM_GetById(vActn.ExHU[vJ]), vActn, null);if (vActn.InitFnc)vActn.InitFnc(vActn);} 
gVAM_MAId = null;} 

function VAM_ChangeFieldState(pAO, pEvalRes){if ((pEvalRes == -1) || gVAM_InFC)return;var vS = pEvalRes == 1; if (pAO.GetChild){var vDone = false;for (var vI = 0; !vDone; vI++){vFld = pAO.GetChild(pAO.IDToChg, vI, 0);if (vFld != null)VAM_CFSBody(pAO, vFld, vS);elsevDone = true;}}else 
{VAM_CFSBody(pAO, pAO.ChgFld, vS);if (pAO.CWC){var vPFld = pAO.ChgFld.parentElement != null ? pAO.ChgFld.parentElement : pAO.ChgFld.parentNode;if (vPFld.id == "") 
VAM_CFSBody(pAO, vPFld, vS);}}
if ((pAO.ChgFld.value != null) && (pAO.CT_Val != pAO.CF_Val))pAO.ChgFld.value = vS ? pAO.CT_Val : pAO.CF_Val;if ((pAO.ChgFld.innerHTML != null) && (pAO.CT_Html != pAO.CF_Html))VAM_SetInnerHTML(pAO.ChgFld, vS ? pAO.CT_Html : pAO.CF_Html);if (pAO.CT_URL != pAO.CF_URL)if ((pAO.ChgFld.tagName == "A") && (pAO.ChgFld.href != null))pAO.ChgFld.href = vS ? pAO.CT_URL : pAO.CF_URL;else if (pAO.ChgFld.src != null)pAO.ChgFld.src = vS ? pAO.CT_URL : pAO.CF_URL;if (pAO.CT_OName != null)if (vS)eval('pAO.ChgFld.' + pAO.CT_OName + '=' + pAO.CT_OVal + ';'); elseeval('pAO.ChgFld.' + pAO.CF_OName + '=' + pAO.CF_OVal + ';');if (pAO.CT_O2Name != null)if (vS)eval('pAO.ChgFld.' + pAO.CT_O2Name + '=' + pAO.CT_O2Val + ';'); elseeval('pAO.ChgFld.' + pAO.CF_O2Name + '=' + pAO.CF_O2Val + ';');if (pAO.RunFnc != null){pAO.RunFnc(pAO, pAO.ChgFld, vS);}
if (!gVAM_Init) 
{ 
if (pAO.VGrp != null)VAM_ValidateGroup(pAO.VGrp, false); else if (pAO.ValC != null) 
{gVAM_InFC = true;if (pAO.ChgFld.multiseg) 
VAM_ValMSDE(pAO.ChgFld.id) 
elseVAM_FieldChanged(pAO.ChgFld.id);gVAM_InFC = false;}
if (window.VAM_CalcAll) 
VAM_CalcAll();VAM_FixAbsPos(true);} 
} 

function VAM_CFSBody(pAO, pFld, pS){
if (pAO.CT_Vis != pAO.CF_Vis){var vVis = pS ? pAO.CT_Vis : pAO.CF_Vis;if (vVis){pFld.style.visibility = "inherit";if (!pAO.InvPS)pFld.style.display = pAO.ODspl;}else{pFld.style.visibility = "hidden";if (!pAO.InvPS)pFld.style.display = "none";}}
if ((pFld.disabled != null) && (pAO.CT_Enab != pAO.CF_Enab)){var vEnab = pS ? pAO.CT_Enab : pAO.CF_Enab;pFld.disabled = !vEnab;}
if ((pFld.readOnly != null) && (pAO.CT_RO != pAO.CF_RO)){pFld.readOnly = pS ? pAO.CT_RO : pAO.CF_RO;}
if ((pAO.CT_Css != pAO.CF_Css) && (pFld.className != null)){var vCss = pS ? pAO.CT_Css : pAO.CF_Css;pFld.className = vCss;}if ((pAO.CT_Chk != pAO.CF_Chk) && pFld.checked != null)pFld.checked = pS ? pAO.CT_Chk: pAO.CF_Chk;} 

function VAM_DoMultiAction(pAO, pEvalRes){for (var vI = 0; vI < pAO.Actions.length; vI++){
var vActn = pAO.Actions[vI];if (vActn.Enabled && vActn.ActnFnc){if (vActn.CanRun && !vActn.CanRun(vActn))return;vActn.ActnFnc(vActn, pEvalRes);}} 
} 

function VAM_RunAllFSC(pAll){if (this.gVAMActions == null) 
return;for (var vI = 0; vI < gVAMActions.length; vI++){var vAO = gVAMActions[vI];if ((vAO.VT == "FS") && (vAO.AutoRun || pAll))VAM_DoAction(vAO);} 
} 
