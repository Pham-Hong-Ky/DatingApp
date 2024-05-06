import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, CanDeactivateFn, RouterStateSnapshot, UrlTree } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export const PreventUnsaveChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
    if(component.editForm.dirty){
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
    }
    return true;
  }

