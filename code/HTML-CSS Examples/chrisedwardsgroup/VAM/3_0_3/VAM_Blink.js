// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


var gVAM_BlinkObjCnt = 0;var gVAM_BlinkState = false;var gVAM_BlinkTimerID = 0;function VAM_StartBlink(pAO){if (gSupportsSetInterval && (pAO.Blnk) && (pAO.BlinkCnt == 0) && (pAO.BlinkFnc != null)){pAO.BlinkCnt = gVAMSubmitEvent ? gVAMBlinkOnSubmit : gVAMBlinkOnChange;if (pAO.BlinkCnt == 0) return;gVAM_BlinkObjCnt++;if (gVAM_BlinkObjCnt == 1){
gVAM_BlinkState = false;gVAM_BlinkTimerID = window.setInterval("JavaScript: VAM_DoBlink();", gVAMBlinkTime);}else if (gVAM_BlinkState) 
pAO.BlinkFnc(pAO, true);}} 

function VAM_StopBlink(pAO){if (pAO.BlinkCnt != 0){pAO.BlinkFnc(pAO, false); gVAM_BlinkObjCnt--;pAO.BlinkCnt = 0;if (gVAM_BlinkObjCnt == 0){window.clearInterval(gVAM_BlinkTimerID);gVAM_BlinkTimerID = 0;}}} 

function VAM_DoBlink(){gVAM_BlinkState = !gVAM_BlinkState;for (var vActnID = 0; vActnID < gVAMActions.length; vActnID++){vAO = gVAMActions[vActnID];if ((vAO.BlinkCnt != null) && (vAO.BlinkCnt != 0)){if ((vAO.BlinkCnt > 0) && !gVAM_BlinkState) 
{if (vAO.BlinkCnt == 1){VAM_StopBlink(vAO);continue;}elsevAO.BlinkCnt--;}vAO.BlinkFnc(vAO, gVAM_BlinkState);}} 
} 

function VAM_Blink(pAO, pBlink){if ((pAO.BlnkCss != "") && (pAO.ErrFld.className != null))pAO.ErrFld.className = pBlink ? pAO.BlnkCss : pAO.OrigCss;else if (pAO.ErrFld.style != null) 
pAO.ErrFld.style.color = pBlink ? '' : pAO.OrigColor; if ((pAO.ImgErrFld != null) && (pAO.ImgErrFld.style != null))pAO.ImgErrFld.style.visibility = pBlink ? "hidden" : "inherit";} 
