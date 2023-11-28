import { throwError } from 'rxjs';



export abstract class BaseService {

    constructor() { }

    protected handleError(error: any) {
    const applicationError = error.headers.get('Application-Error');

    // either applicationError in header or model error in body
    if (applicationError) {
      throwError(() => new Error(applicationError))
    }

    let modelStateErrors = '';
    const serverError = error.json();

    if (!serverError.type) {
      for (const key in serverError) {
        if (serverError[key]) {
          modelStateErrors += serverError[key] + '\n';
        }
      }
    }

    modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;
    throwError(() => new Error(modelStateErrors || 'Server error'))
  }
}
