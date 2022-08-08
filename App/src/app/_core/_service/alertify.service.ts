import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
declare let Swal: any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {
  public $swal = Swal;
  constructor(private trans: TranslateService) { }
  private Toast = Swal.mixin({
    toast: true,
    position: 'bottom-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    onOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer);
      toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
  });
  showLoading(timer = 2000) {
    {
      Swal({
        title: this.trans.instant('Now loading'),
        allowEscapeKey: false,
        allowOutsideClick: false,
        timer,
        onOpen: () => {
          Swal.showLoading();
        }
      }).then(
        () => { },
        (dismiss) => {
          if (dismiss === 'timer') {
            Swal({
              title: this.trans.instant('Finished!'),
              type: 'success',
              timer: 2000,
              showConfirmButton: false
            });
          }
        }
      );
    }
  }
  confirm(title: string, message: string, okCallback: () => void) {
    Swal.fire({
      title,
      // text: message,
      html: message,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: this.trans.instant('Yes'),
      cancelButtonText: this.trans.instant('No!')
    }).then((result) => {
      if (result.value) {
        okCallback();
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire(
          this.trans.instant('Cancelled'),
          this.trans.instant('Your imaginary file is safe :)'),
          this.trans.instant('error')
        );
      }
    });
  }
  confirm2(title: string, message: string, okCallback: () => void, cancelCallback: () => void) {
    Swal.fire({
      title,
      html: message,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: this.trans.instant('Yes'),
      cancelButtonText: this.trans.instant('No!')
    }).then((result) => {
      if (result.value) {
        okCallback();
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        cancelCallback();
      }
    });
  }
  confirm4(confirmButtonText = 'Yes',cancelButtonText = 'No', title: string, message: string, okCallback: () => void, cancelCallback: () => void ) {
    Swal.fire({
      title,
      html: message,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: confirmButtonText,
      cancelButtonText: cancelButtonText
    }).then((result) => {
      if (result.value) {
        okCallback();
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        cancelCallback();
      }
    });
  }
  confirm5(confirmButtonText = 'Yes',cancelButtonText = 'No', title: string, message: string, okCallback: () => void, cancelCallback: () => void ) {
    Swal.fire({
      title,
      html: message,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: confirmButtonText,
      cancelButtonText: cancelButtonText
    }).then((result) => {
      if (result.value) {
        okCallback();
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        cancelCallback();
      }
    });
  }
  confirm3(title: string, message: string, confirmButtonText: string, cancelButtonText: string, okCallback: () => void, cancelCallback: () => void) {
    Swal.fire({
      title,
      html: message,
      icon: 'success',
      showCancelButton: true,
      confirmButtonText: confirmButtonText,
      cancelButtonText: cancelButtonText,
      allowOutsideClick: false
    }).then((result) => {
      if (result.value) {
        okCallback();
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        cancelCallback();
      }
    });
  }
  valid(title: string, message: string): Promise<boolean> {
    return new Promise((res, rejects) => {
      Swal.fire({
        title,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: this.trans.instant('Yes'),
        cancelButtonText: this.trans.instant('No')
      }).then((result) => {
        if (result.value) {
          res(true);
        } else if (result.dismiss === Swal.DismissReason.cancel) {
          rejects(false);
        }
      });
    });
  }
  validation(title: string, message: string) {
    Swal.fire(title, message, 'warning');
  }

  success(message: string, noToast?: boolean) {
    if (!noToast) {
      this.Toast.fire({
        icon: 'success',
        title: message
      });
    } else {
      Swal.fire(this.trans.instant('SUCCESS_ALERT_MESSAGE'), message, 'success');
    }
  }
  errorBackToLogin(message: string, btnText: string, callBack: any, showCancelButton = false, errorCallBack = () => {}) {
    Swal.fire({
      text: message,
      icon: 'error',
      showCancelButton: showCancelButton,
      allowOutsideClick: false,
      confirmButtonText: `<i class="fa fa-backward"></i> ${btnText}`,
      cancelButtonText: this.trans.instant('No')
    }).then((result) => {
      if (result.value) {
       callBack();
      } else {
        errorCallBack()
      }
    });
  }
  error(message: string, noToast?: boolean) {
    if (!noToast) {
      this.Toast.fire({
        icon: 'error',
        title: message
      });
    } else {
      Swal.fire(this.trans.instant('ERROR_ALERT_MESSAGE'), message, 'error');
    }
  }

  warning(message: string, noToast?: boolean) {
    if (!noToast) {
      this.Toast.fire({
        icon: 'warning',
        title: message
      });
    } else {
      Swal.fire(this.trans.instant('WARNING_ALERT_MESSAGE'), message, 'warning');
    }
  }

  message(message: string, noToast?: boolean) {
    if (!noToast) {
      this.Toast.fire({
        icon: 'info',
        title: message
      });
    } else {
      Swal.fire(this.trans.instant('INFO_ALERT_MESSAGE'), message, 'info');
    }
  }
}
