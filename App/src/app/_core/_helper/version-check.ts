import { environment } from "src/environments/environment";
import { VersionCheckService } from "../_service/version-check.service";

export function versionCheck(versionCheckService: VersionCheckService) {
  return () =>
    new Promise((resolve, reject) => {
          //console.log('refresh token on app start up');
          versionCheckService.initVersionCheck(environment.versionCheckURL);
          resolve(true);
      });
}

