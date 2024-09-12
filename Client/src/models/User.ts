export interface IUser{
    Id:number;
    Login:string;
    Name:string;
    Email:string;
    IsAdmin:boolean;
}
export interface IUserLogin extends IUser{
    Password:string;
}