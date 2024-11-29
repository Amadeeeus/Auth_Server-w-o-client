import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private isLogined = false;
    private isClickRegister = false;
    private isClickLogin = false;
    private isButtons = true;
    public email: string="";

    getIsLogined() {
        return this.isLogined;
    }

    setIsLogined(value: boolean) {
        this.isLogined = value;
    }

    getIsClickRegister() {
        return this.isClickRegister;
    }

    setIsClickRegister(value: boolean) {
        this.isClickRegister = value;
    }

    getIsClickLogin()
    {
        return this.isClickLogin;
    }

    setIsClickLogin(value: boolean)
    {
        this.isClickLogin = value;
    }
    setButtons(value:boolean)
    {
        this.isButtons = value;
    }
    getButtons()
    {
        return this.isButtons;
    }

}
