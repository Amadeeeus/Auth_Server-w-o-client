import {Component, OnInit} from "@angular/core";
import {FormsModule} from "@angular/forms";
import {AuthService} from "./app.components";

@Component({
    selector: "logined",
    standalone: true,
    //styleUrls: ["./main.css"],
    template:
    `
        <h1>Hello, {{this.username}}</h1>
    `
})

export class LoginComponent
{
    username: string ='';
    constructor(public authService: AuthService)
    {
        this.username = authService.email;
    }
}