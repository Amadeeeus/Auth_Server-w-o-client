import {Component,} from "@angular/core";
import {FormsModule} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {User} from "./user.model";
import {AuthService} from "./app.components";


@Component({
    selector:"register",
    standalone:true,
    imports:[FormsModule],
    styleUrls:["../styles/register.css"],
    template:
        `
            <form (ngSubmit)="onSubmit()" #form = "ngForm" class="registerForm" action="" method="post">
                <input [(ngModel)]="formData.firstName" class="input" type="text">
                <input [(ngModel)]="formData.lastName" class="input" type="text">
                <input [(ngModel)]="formData.email" class="input" type="text" placeholder="email">
                <input [(ngModel)]="formData.password" class="input" id="password" type="password">
                <button class="submitbutton" type="submit">Регистрация:{{formData.email}}</button>
            </form>
            <button class="mainbutton" (click)="toggleReset()" >Сброс</button>
            <button class="mainbutton" (click)="togglePassword()">Показать пароль</button>
            <button class="mainbutton" (click)="toggleBack()">Назад</button>
        `
})


export class AppRegister {
    formData = {
        firstName: '',
        lastName: '',
        email: '',
        password: ''
    }

    user: User = new User();
    constructor(private http: HttpClient,public authService: AuthService) {
    }

    onSubmit() {
        this.authService.setIsClickLogin(false);
        this.http.post<User>('/api/register', this.formData).subscribe({
            next: (res) =>
            {
                this.user =res;
                if(this.user)
                {
                    this.authService.setIsLogined(true)
                    this.authService.setButtons(true)
                    this.authService.setIsClickRegister(false);
                }
            },
            error:(err)=>
            {
              console.error("Ошибка",err);
              this.authService.setIsClickRegister(false);
            }
        });
    }

    togglePassword()
    {
        const field = document.querySelector('#password')as HTMLInputElement;
        if(field.type==='password')
        {
            field.type='text';
        }
        else
        {
            field.type='password'
        }
    }

    toggleReset()
    {
        this.formData.email = "";
        this.formData.password = "";
        this.formData.firstName = "";
        this.formData.lastName = "";
    }
    toggleBack()
    {
        this.authService.setIsClickRegister(false);
        this.authService.setButtons(true);
    }
}