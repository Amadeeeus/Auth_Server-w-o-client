import {Component} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {User} from "./user.model";
import {FormsModule, NgForm} from "@angular/forms";
import {AuthService} from "./app.components";

@Component({
    selector: "login",
    standalone: true,
    imports:[FormsModule],
    styleUrls:["../styles/login.css"],
    template:
        `
        <form  class="form" (ngSubmit)="onSubmit()" #form ="ngForm" action="">
            <div class="tr">
            <input class="input" [(ngModel)]="formData.email" type="text">
            <input class="input" [(ngModel)]="formData.password" id="password" type="password">
            <button class="submitbutton" type="submit">Войти</button>
            </div>
        </form>          
        <div class="buttons">
        <button class="mainbutton" (click)="togglePassword()" type="button">Показать пароль</button>
        <button class="mainbutton" (click)="toggleReset(NgForm)">Сброс</button>
        <button class="mainbutton" (click)="toggleBack()">Назад</button>
        </div>
        `
})

export class AppLogin {

    formData =
        {
            email: "",
            password: "",
        }
    constructor(private http: HttpClient, public auth: AuthService)
    {
    }

    user:User = new User();
    onSubmit()
    {
        this.http.post<User>('http://localhost:5296/api/auth/login', this.formData).subscribe(
            {
                next: (res) => {
                    if (res)
                    {
                        this.auth.setIsLogined(true)
                        this.auth.setIsClickLogin(false);
                        this.auth.setButtons(true)
                        this.auth.email = this.formData.email;
                    }
                }
            })
    }

    togglePassword()
    {
        const passwordField = document.querySelector('#password')as HTMLInputElement;
        if(passwordField.type==='password')
        {
            passwordField.type='text';
        }
        else
        {
            passwordField.type='password';
        }
    }

    toggleReset(form:NgForm)
    {
        this.formData.email = "";
        this.formData.password = "";
        form.resetForm();
    }

    toggleBack()
    {
        this.auth.setIsClickLogin(false);
        this.auth.setButtons(true);
    }

    protected readonly NgForm = NgForm;
}