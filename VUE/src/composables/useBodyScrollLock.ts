import { onBeforeUnmount, watch, type Ref } from 'vue';

/**
 * Locks page scroll when `locked` is true.
 * Uses the "position: fixed" technique to reliably prevent background scroll on mobile.
 */
export function useBodyScrollLock(locked: Ref<boolean>) {
    let lockedScrollY = 0;
    let lockedPath = '';

    const setLocked = (isLocked: boolean) => {
        if (typeof window === 'undefined') return;

        const body = document.body;
        const html = document.documentElement;

        if (isLocked) {
            lockedScrollY = window.scrollY || 0;
            lockedPath = window.location.pathname + window.location.search + window.location.hash;

            // Tailwind-friendly approach: still uses inline styles (reliable),
            // but keeps the calling component clean.
            html.style.overflow = 'hidden';
            body.style.overflow = 'hidden';
            body.style.position = 'fixed';
            body.style.top = `-${lockedScrollY}px`;
            body.style.left = '0';
            body.style.right = '0';
            body.style.width = '100%';
            return;
        }

        html.style.overflow = '';
        body.style.overflow = '';
        body.style.position = '';
        body.style.top = '';
        body.style.left = '';
        body.style.right = '';
        body.style.width = '';

        const currentPath = window.location.pathname + window.location.search + window.location.hash;
        // If navigation happened while locked, don't restore the old scroll.
        if (currentPath === lockedPath) {
            window.scrollTo(0, lockedScrollY);
        } else {
            window.scrollTo(0, 0);
        }
    };

    watch(locked, (value) => setLocked(value), { immediate: true });
    onBeforeUnmount(() => setLocked(false));

    return { setLocked };
}
