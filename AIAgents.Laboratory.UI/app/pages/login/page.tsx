"use client";

import { useMsal } from "@azure/msal-react";
import { Button } from "@heroui/react";

import { loginRequest } from "@/auth/authConfig";
import "./styles.css";

export default function LoginPage() {
	const { instance } = useMsal();

	const handleLogin = () => {
		instance.loginRedirect(loginRequest).catch((e) => {
			console.error(e);
		});
	};

	return (
		<div className="login-background">
			<div className="login-content">
				<div className="bg-white/20 backdrop-blur-md rounded-2xl p-8 border border-white/30 shadow-2xl">
					<h1 className="text-white text-4xl font-bold mb-6 text-center drop-shadow-lg">
						AI Agents Laboratory
					</h1>
					<p className="text-white/90 text-lg mb-8 text-center drop-shadow-md">
						Welcome to the future of AI development
					</p>
					<div className="flex flex-wrap items-center gap-2 md:flex-row">
						<Button
							className="bg-linear-to-tr from-pink-500 to-yellow-500 text-white shadow-lg p-4 font-bold"
							radius="full"
							onPress={handleLogin}
						>
							Sign In with SSO
						</Button>
					</div>
				</div>
			</div>
		</div>
	);
}
