export class SystemConstant {
    public static ROOT_LEVEL = 1;
    public static BUILDING_LEVEL = 2;
    public static LINE_LEVEL = 3;
}
export class BOM_TAB_Constant {
  public static Move = 'Move';
  public static Feeding = 'Feeding';
  public static Immunization = 'Immunization';
  public static Treatment = 'Treatment';
  public static Disinfection = 'Disinfection';
  public static VectorControl ='VectorControl';
}
export class BIO_SECURITY_TAB_Constant {
  public static Detail = 'Detail';
  public static Pen = 'Pen';
  public static Pig = 'Pig';
  public static Record = 'Record';

}
export class INVENTORY_TAB_Constant {
  public static Detail = 'Detail';
  public static ChangeThing = 'Change Thing';
  public static ChangeMaterial = 'Change Material';
  public static Scrap = 'Scrapn';

}
export class ACCEPTANCE_TAB_Constant {
  public static Detail = 'Detail';
  public static Check = 'Check';
  public static CheckIn = 'Check In';
  public static Inspection = 'Inspection';

}
export class BIO_SECURITY_MASTER_PIG_TYPE_Constant {
  public static Sow = 'Sow';
  public static Boar = 'Boar';
  public static NewBoar = 'New Boar';
  public static Gilt = 'Gilt';
  public static Suckling = 'Suckling';
  public static Pig = 'Pig';
  public static Finisher = 'Finisher';
  public static Nursery = 'Nursery';
  public static Grower = 'Grower';

}
export class REPAIR_TAB_Constant {
  public static Detail = 'Detail';
  public static Record = 'Record';
}
export class SALE_ORDER_TAB_Constant {
  public static Detail = 'Detail';
  public static CheckOut = 'Check Out';
}
export class REQUISITION_TAB_Constant {
  public static Detail = 'Detail';
  public static Feed = 'Feed';
  public static Material = 'Material';
  public static Medicine = 'Medicine';
  public static Thing = 'Thing';
}
export class PIG_DISEASE_TAB_Constant {
  public static Culling = 'Culling';
  public static Detail = 'Detail';
}

export class PIG_HOUSE_CLEANING_TAB_Constant {
  public static Plan = 'Plan';
  public static Detail = 'Detail';
}
export class PIG_FARM_VECTOR_TAB_Constant {
  public static Plan = 'Plan';
  public static Detail = 'Detail';
}
export class PIG_SETTING_TAB_Constant {
  public static Code = 'Code';
  public static Detail = 'Detail';
  public static Testing = 'Testing';
  public static Genetic = 'Genetic';
  public static Pedigree = 'Pedigree';

}
export class RECORD_TAB_Constant {
  public static Detail = 'Detail';
  public static Move = 'Move';
  public static Feeding = 'Feeding';
  public static Death = 'Death';
  public static Culling = 'Culling';
  public static EarTag = 'EarTag';
  public static Weighing ='Weighing';
}
export class MessageConstants {
  public static SYSTEM_ERROR_MSG = 'An error occurred while connecting to the server';
  public static CONFIRM_LOCK_MSG = 'Are you sure you want to lock this account?';
  public static CONFIRM_UNLOCK_MSG = 'Are you sure you want to unlock this account?';

  public static CONFIRM_LOCK_STATUS_MSG = 'Are you sure you want to lock this record?';
  public static CONFIRM_UNLOCK_STATUS_MSG = 'Are you sure you want to unlock this record?';
  public static CONFIRM_STATUS_TITLE_MSG = 'Lock/Unlock record?';

  public static CONFIRM_FEEDBACK_STATUS_MSG = 'Are you sure you want to reply to this message??';
  public static CONFIRM_UNFEEDBACK_STATUS_MSG = 'Are you sure you want to change the status of this message??';
  public static CONFIRM_FEEDBACK_STATUS_TITLE_MSG = 'Reply/Unresponsive to mail?';

  public static CONFIRM_DELETE_MSG = 'Are you sure you want to delete this record?';
  public static CONFIRM_DELETE_RANGE_MSG = 'Are you sure you want to delete these records?';
  public static CONFIRM_LOCK_TITLE_MSG = 'Lock/Unlock account?';
  public static CONFIRM_TITLE_MSG = 'Delete record?';
  public static CONFIRM_DELETE_RANGE_TITLE_MSG = 'Delete multiple records?';
  public static CONFIRM_PAY_MSG = 'Are you sure you want to pay this?';
  public static CONFIRM_SET_DEFAULT_MSG = 'Are you sure you want to default to this record?';
  public static CONFIRM_SET_IS_HOME_MSG = 'Are you sure you want to display this record on the homepage?';
  public static CONFIRM_SET_NOT_IS_HOME_MSG = 'Are you sure you want to undisplay this record on the homepage?';
  public static CONFIRM_PUBLISH_POST = 'Are you sure you want to publish this article??';
  public static LOGIN_AGAIN_MSG = 'Your login session is over. Please login again.';
  public static CREATED_OK_MSG = 'Create Successfully';
  public static UPDATED_OK_MSG = 'Update successfully';
  public static DELETED_OK_MSG = 'Delete successfully';
  public static LOCKED_OK_MSG = 'Locked successfully';
  public static UNLOCKED_OK_MSG = 'Unlock successfully';
  public static SET_DEFAULT_OK_MSG = 'Default set success';
  public static SET_IS_HOME_OK_MSG = 'Set homepage display successfully';
  public static SET_NOT_IS_HOME_OK_MSG = 'Successfully undisplaying homepage';
  public static FORBIDDEN = 'You are blocked from accessing';
  public static CANNOT_EDIT_MULTIPLE = 'You cannot edit more than 1 record.';
  public static NOT_CHOOSE_ANY_RECORD = 'You must select at least one record.';
  public static UPLOAD_OK_MSG = 'Upload successful';
  public static REQUIRED_ERROR_MSG = 'Data cannot be empty';
  public static RELOAD_MENU = 'Reload menu';
  public static SELECT_RECORD = 'Please select at least 1 record to delete!';

  public static SEND_MAIL_OK_MSG = 'Email was sent. Please check your email';
  public static SEND_MAIL_FAILED_MSG = 'Failure sending email error!';
  public static CREATE_TITLE = 'Create record';
  public static CREATE_MESSAGE = 'Are you sure you want to create this record?';
  public static UPDATE_TITLE = 'Update record';
  public static UPDATE_MESSAGE = 'Are you sure you want to update this record?';
  public static DELETE_TITLE = 'Delete record';
  public static DELETE_MESSAGE = 'Are you sure you want to delete this record?';
  public static CANCEL_MESSAGE = 'Your data is safe!';
  public static SERVER_ERROR = 'Server Error!';
  public static EXIST_MESSAGE = 'The key already exist!';
  public static EXIST_USERNAME_MESSAGE = 'The username already exist!';
  public static CHOOSE_FARM_MESSAGE = 'Please choose a farm!';
  public static SELECT_ORDER_MESSAGE = 'Please select a order!';
  public static VALID_CHANGE_PASSWORD_MSG = "The new password and confirm password are empty!";
  public static CONFIRM_CHANGE_PASSWORD_MSG = "Password and Confirm Password Validation";
  public static YES_MSG = "Yes";
  public static NO_MSG = "No";

}
export class ActionConstants {
  public static ADD = 'add';
  public static EDIT = 'edit';
  public static VIEW = 'getall';
  public static DELETE_RANGE = 'deleterange';
  public static DELETE = 'delete';
  public static EDIT_TITLE = 'Edit';
  public static ADD_TITLE = 'Add new';
  public static Add = 'add';
  public static Edit = 'edit';

}
export class ImagePathConstants {
  public static NO_IMAGE = '/assets/images/default-avatar-male.png';
  public static NO_IMAGE_ROOM = '/assets/images/no-img.jpg';
  public static NO_IMAGE_COMPONENT = '/assets/images/pages/content-img-4.jpg';
  public static NO_IMAGE_ACTION_COMPONENT = '/assets/images/no-image.png';
}
