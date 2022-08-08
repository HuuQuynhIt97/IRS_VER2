import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root', })
export class FunctionUtility {
    // Converting "String" to javascript "File Object"
    convertToFile(listFile: string[], urlFolder: string, file: File[]) {
        // ***Here is the code for converting "String" to "Base64".***
        listFile.forEach(element => {
            if (element !== '') {
                let url = urlFolder + element;
                const toDataURL = url => fetch(url)
                    .then(response => response.blob())
                    .then(blob => new Promise((resolve, reject) => {
                        const reader = new FileReader();
                        reader.onloadend = () => resolve(reader.result);
                        reader.onerror = reject;
                        reader.readAsDataURL(blob);
                    }));

                // *** Calling both function ***
                toDataURL(url).then(dataUrl => {
                    let fileData = dataURLtoFile(dataUrl, element);
                    file.push(fileData);
                });

                // ***Here is code for converting "Base64" to javascript "File Object".***
                function dataURLtoFile(dataurl, filename) {
                    let arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
                        bstr = atob(arr[1]),
                        n = bstr.length,
                        u8arr = new Uint8Array(n);
                    while (n--) {
                        u8arr[n] = bstr.charCodeAt(n);
                    }
                    return new File([u8arr], filename, { type: mime });
                }
            }
        });
    }
    // End Converting "String" to javascript "File Object"
}