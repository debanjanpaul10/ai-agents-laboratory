import { useMsal } from "@azure/msal-react";
import { Button } from "@heroui/react";

import { loginRequest } from "@auth/authConfig";
import styles from "@pages/login/styles.module.css";
import { LoginPageConstants } from "@helpers/constants";

export default function LoginPage() {
	const { instance } = useMsal();

	const handleLogin = () => {
		instance.loginRedirect(loginRequest).catch((e) => {
			console.error(e);
		});
	};

	return (
		<div className={styles.loginBackground}>
			<div className={styles.loginContent}>
				{/* MOBILE VERSION */}
				<div
					className={`${styles.mobileOnly} bg-white/20 backdrop-blur-md rounded-2xl p-6 md:p-8 border border-white/30 shadow-2xl max-w-md w-full mx-auto md:mx-0`}
				>
					<div className="block mb-8 text-center">
						<img
							src="/images/icon.png"
							alt="AI Agents Laboratory Logo"
							className="w-20 h-20 object-contain drop-shadow-lg mx-auto mb-4"
						/>
						<h1 className="text-white text-2xl font-bold mb-4 drop-shadow-lg">
							{LoginPageConstants.HeaderText}
						</h1>
						<p className="text-white/90 text-base drop-shadow-md">
							{LoginPageConstants.Subtext}
						</p>
					</div>
					<div className="flex justify-center justify-center w-full">
						<Button
							className="bg-linear-to-tr from-pink-500 to-yellow-500 text-white shadow-lg font-bold w-full md:w-auto"
							radius="full"
							onPress={handleLogin}
						>
							<span className="p-2 text-center">
								{LoginPageConstants.LoginButton}
							</span>
						</Button>
					</div>
				</div>

				{/* DESKTOP VERSION */}
				<div
					className={`${styles.desktopOnly} bg-white/20 backdrop-blur-md rounded-2xl p-6 md:p-8 border border-white/30 shadow-2xl max-w-md w-full mx-auto md:mx-0`}
				>
					<div className="block mb-8 text-center">
						<h1 className="text-white text-3xl font-bold mb-6 text-center drop-shadow-lg">
							{LoginPageConstants.HeaderText}
						</h1>
						<p className="text-white/90 text-lg mb-8 text-center drop-shadow-md">
							{LoginPageConstants.Subtext}
						</p>
					</div>

					<div className="flex justify-center justify-center w-full">
						<Button
							className="bg-linear-to-tr from-pink-500 to-yellow-500 text-white shadow-lg font-bold w-full md:w-auto"
							radius="full"
							onPress={handleLogin}
						>
							<span className="p-2 text-center">
								{LoginPageConstants.LoginButton}
							</span>
						</Button>
					</div>
				</div>
			</div>
		</div>
	);
}
