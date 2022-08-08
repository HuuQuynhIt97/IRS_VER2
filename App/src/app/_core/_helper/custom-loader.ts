import { TranslateLoader } from "@ngx-translate/core";
import { Observable, of } from "rxjs";

export class CustomLoader implements TranslateLoader {
  getTranslation(lang: string): Observable<any> {
    const languages = JSON.parse(localStorage.getItem('languages'));
    if (languages) {
      return of(languages["languages"]);
    } else {
      return of({});
    }
  }
}
