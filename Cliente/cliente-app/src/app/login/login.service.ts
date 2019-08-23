import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class LoginService {

  constructor(private http: HttpClient) {
  }

  login(username:string, password:string):Observable<any> {
     var peticion = "[+LOGIN][+USER]"+username+"[-USER][+PASS]"+password+"[-PASS][-LOGIN]";
    return this.http.post('https://localhost:44313/api/values', {"data":peticion});
  }
  getProductos(): Observable<any>{
    return this.http.get('https://localhost:44313/api/values');
}

}