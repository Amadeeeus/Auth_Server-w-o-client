import {Component} from "@angular/core";
import {FormsModule} from "@angular/forms";
import {AppRegister} from "./app.register";
import {AuthService} from "./app.components";
import {NgIf} from "@angular/common";
import {AppLogin} from "./app.login";
import {LoginComponent} from "./app.logined";
import {HttpClientModule} from "@angular/common/http";

@Component
({
    imports: [FormsModule, AppRegister, NgIf, AppLogin, LoginComponent, HttpClientModule],
    selector: "app",
    standalone: true,
    styleUrls: ["../styles/main.css"],
    template:
        `<div class="main">
            <div class="logined">
                <logined *ngIf="authService.getIsLogined()"></logined>
            </div>
            <div class="workflow">
                <register *ngIf="authService.getIsClickRegister()"></register>
                <login *ngIf="authService.getIsClickLogin()"></login>
                <div class="buttons" *ngIf="authService.getButtons()">
                <button class="mainbutton" (click)="toggleLogin()">Войти</button>
                <button class="mainbutton" (click)="toggleRegister()">Регистрация</button>
                    </div>
            </div>
        </div>
        `
})

export class AppComponent
{
    constructor(public authService: AuthService)
    {

    }
    toggleRegister():void
    {
     this.authService.setIsClickRegister(true);
     this.authService.setButtons(false)
     this.authService.setIsClickLogin(false)
    }
    toggleLogin():void
    {
        this.authService.setIsClickLogin(true);
        this.authService.setButtons(false)
        this.authService.setIsClickRegister(false);
    }
}