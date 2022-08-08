import { CookieService } from 'ngx-cookie-service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable()
export class VersionCheckService {
  // this will be replaced by actual hash post-build.js
  private currentHash = '{{POST_BUILD_ENTERS_HASH_HERE}}';
  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    ) {
      //console.log(this.currentHash)
     }
  /**
  * Checks in every set frequency the version of frontend application
  * @param url
  * @param {number} frequency - in milliseconds, defaults to 30 minutes
  */
  public initVersionCheck(url, frequency = 1000 * 60 * 30) {
    setInterval(() => {
      this.checkVersion(url);
    }, frequency);
    this.checkVersion(url);
  }
  /**
  * Will do the call and check if the hash has changed or not
  * @param url
  */
  private checkVersion(url) {
    // timestamp these requests to invalidate caches
    this.http.get(url + '?t=' + new Date().getTime())
      .subscribe(
        (response: any) => {
         // console.log('current Hash',this.currentHash);
          const hash = response.hash;
        //  console.log('new Hash',hash);
          const hashChanged = this.hasHashChanged(this.currentHash, hash);
          // If new version, do something
         // console.log('If new version, do something',hashChanged);

          if (hashChanged) {
           // console.log(`There is a new update version ${response.version}` + ' current Hash',this.currentHash);
            localStorage.setItem('version', response.version )
            this.currentHash = hash;
            // this.cookieService.deleteAll('/');
            window.location.reload();
            // ENTER YOUR CODE TO DO SOMETHING UPON VERSION CHANGE
            // for an example: location.reload();
          }
          this.currentHash = hash;
          // store the new hash so we wouldn't trigger versionChange again
          // only necessary in case you did not force refresh
        },
        (err) => {
          console.error(err, 'Could not get version');
        }
      );
  }
  /**
  * Checks if hash has changed.
  * This file has the JS hash, if it is a different one than in the version.json
  * we are dealing with version change
  * @param currentHash
  * @param newHash
  * @returns {boolean}
  */
  private hasHashChanged(currentHash, newHash) {
    if (!currentHash || currentHash === '{{POST_BUILD_ENTERS_HASH_HERE}}') {
      return false;
    }
    return currentHash !== newHash;
  }
}
