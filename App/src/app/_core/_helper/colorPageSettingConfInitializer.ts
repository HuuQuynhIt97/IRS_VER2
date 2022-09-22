import { ShoeGlueService } from "../_service/setting/shoe-glue.service";

export function colorPageSettingConfInitializer(
  serviceShoeGlue: ShoeGlueService
  ) {
    return () =>
      new Promise((resolve, reject) => {
            serviceShoeGlue.getMenuPageSetting().subscribe(data => {
                localStorage.setItem('colorPageSetting', JSON.stringify(data));
              }).add(resolve);;
        });
}
