// Copyright 2003 - 2005 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Professional Validation And More v3.0.3


function VAM_EvalCallBackCond(pCO){if (!gVAMSubmitEvent){__theFormPostData = '';WebForm_InitCallback(); eval(pCO.Callback);}if (pCO.Action.LastResult != null)return pCO.Action.LastResult; elsereturn -1; } 

function VAM_CBResp(pR, pVID){var vParts = pR.split(String.fromCharCode(20));var vAO = VAM_FindAOById(pVID);vAO.CondResult = parseInt(vParts[0]);vAO.LastResult = vAO.CondResult;vAO.ErrMsg = vParts[1];vAO.SumMsg = vParts[2];VAM_DoValidate(vAO, vAO.CondResult);if (vAO.Ctl != null)VAM_PostValidateFld(VAM_GetById(vAO.Ctl[0].id));} 