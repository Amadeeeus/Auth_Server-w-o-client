import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {User} from "./user.login";

@Injectable()
export class ApiService
{
    private apiUrl: string= '';
    constructor(private http: HttpClient) {}
    Login():Observable<User>
    {
        return this.http.post<User>(`${this.apiUrl}/api/login`, User);
    }
    Register(user:User):Observable<User>
    {
        return this.http.post<User>(`${this.apiUrl}/api/register`, User);
    }

    GetAllUsers(user:User): Observable<User[]>
    {
        return this.http.get<User[]>(`${this.apiUrl}/api/auth`);
    }

    GetUser():Observable<User>
    {
        return this.http.post<User>(`${this.apiUrl}/api/auth`, User);
    }

    PutUser():Observable<User>
    {
        return this.http.put<User>(`${this.apiUrl}/api/auth`, User);
    }

    Delete():Observable<void>
    {
        return this.http.delete<void>(`${this.apiUrl}/api/auth`);
    }

}