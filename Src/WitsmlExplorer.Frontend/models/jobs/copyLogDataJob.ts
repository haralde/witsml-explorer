import LogReference from "./logReference";
import LogObject from "../logObject";
import { LogCurveInfoRow } from "../../components/ContentViews/LogCurveInfoListView";

export interface CopyLogDataJob {
  target: LogReference;
  logCurvesReference: LogCurvesReference;
}

export interface LogCurvesReference {
  serverUrl: string;
  logReference: LogReference;
  mnemonics: string[];
}

export function parseStringToLogCurvesReference(input: string): LogCurvesReference {
  let jsonObject: LogCurvesReference;
  try {
    jsonObject = JSON.parse(input);
  } catch (error) {
    throw new Error("Invalid input given.");
  }
  verifyRequiredProperties(jsonObject);

  return {
    serverUrl: jsonObject.serverUrl,
    logReference: jsonObject.logReference,
    mnemonics: jsonObject.mnemonics
  };
}

function verifyRequiredProperties(jsonObject: LogCurvesReference) {
  const requiredProps = ["serverUrl", "logReference", "mnemonics"];
  const hasRequiredProperties = requiredProps.every((prop) => Object.prototype.hasOwnProperty.call(jsonObject, prop));
  if (!hasRequiredProperties) {
    throw new Error("Missing required fields.");
  }
}

export function createLogCurvesReference(logCurveInfoRows: LogCurveInfoRow[], source: LogObject, serverUrl: string): LogCurvesReference {
  return {
    serverUrl: serverUrl,
    logReference: {
      wellUid: source.wellUid,
      wellboreUid: source.wellboreUid,
      logUid: source.uid
    },
    mnemonics: logCurveInfoRows.map((dataRow) => dataRow.mnemonic)
  };
}

export function createCopyLogDataJob(logCurvesReference: LogCurvesReference, target: LogObject): CopyLogDataJob {
  return {
    logCurvesReference,
    target: {
      wellUid: target.wellUid,
      wellboreUid: target.wellboreUid,
      logUid: target.uid
    }
  };
}
