import { Component, OnInit } from '@angular/core';
import { LoginService } from './login.service';
import {  PruebaEntrada } from './prueba';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  
  constructor(private loginService: LoginService,private prueba: PruebaEntrada, public router: Router) { }
  logeado:boolean

  ngOnInit() {
  }
  logIn(username: string, password: string, event: Event) {
    event.preventDefault(); // Avoid default action for the submit button of the login form

    // Calls service to login user to the api rest
    this.loginService.login(username, password).subscribe(

      res => {
        
        var inicio = this.prueba.imprimirAlgo(res[1]);
        console.log('RESPUESTA: '+ inicio )
        if(inicio===true){
          this.logeado=true;
          this.navigate();
        }else this.logeado=false;
       //console.log(res[1]);
       //PARSER_LUP.INICIAR(res[1]);
      }
    );
  }
  navigate() {
    this.router.navigate(['/Principiante']);
  }
}
