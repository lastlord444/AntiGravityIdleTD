# Known Risks

1. **Scope Creep:** Combining TD with complex idle metas can bloat the project.
   - *Mitigation:* Focus on the core lane defense and basic anti-gravity flip first.
2. **Missing `main` branch:** The `main` branch was not initialized properly.
   - *Mitigation:* `develop` will be the default, and we will branch `main` when v1.0 MVP is stable.
3. **Ghost Files:** `.cs~` files can clutter the workspace and cause confusion since Unity ignores them but Git does not (if not in gitignore).
   - *Mitigation:* Updated `.gitignore` (pending) or strict Git clean discipline.
