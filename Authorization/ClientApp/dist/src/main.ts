import {bootstrapApplication} from "@angular/platform-browser";
import {AppComponent} from "./app/app.main";
import {AppRegister} from "./app/app.register";
import { HttpClientModule } from '@angular/common/http';
import {importProvidersFrom} from "@angular/core";
bootstrapApplication(AppComponent,
    {providers: [importProvidersFrom(HttpClientModule)]}).catch(error => {console.error(error);});
bootstrapApplication(AppRegister).catch(error => {console.error(error);});