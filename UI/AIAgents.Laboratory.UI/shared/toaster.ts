import { addToast } from "@heroui/react";

export function ShowSuccessToaster(message: string) {
    addToast({
        title: message,
        severity: "success",
        variant: "solid",
        classNames: {
            base: "bg-green-600 text-white max-w-md rounded-xl shadow-[0_0_20px_rgba(34,197,94,0.5)] border border-green-400/30",
            wrapper: "p-4",
            content: "text-white",
            title: "text-white font-medium text-base",
            closeButton:
                "text-white hover:bg-white/20 rounded-md transition-all",
            icon: "text-white",
        },
    });
}

export function ShowErrorToaster(message: string) {
    addToast({
        title: message,
        severity: "danger" as any,
        variant: "solid",
        classNames: {
            base: "bg-red-600 text-white max-w-md rounded-xl shadow-[0_0_20px_rgba(220,38,38,0.5)] border border-red-400/30",
            wrapper: "p-4",
            content: "text-white",
            title: "text-white font-medium text-base",
            closeButton:
                "text-white hover:bg-white/20 rounded-md transition-all",
            icon: "text-white",
        },
    });
}
