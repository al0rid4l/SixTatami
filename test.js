function gcd(a, b) {
	return b === 0 ? a : gcd(b, a % b);
}
function EuclideanAlgorithm(a, b) {
	a < b && ([a, b] = [b, a]);
	return b === 0 ? a : EuclideanAlgorithm(b, a % b);
}

function EuclideanAlgorithmN(...args) {
	if (args.length == 2) {
		return EuclideanAlgorithm(args[0], args[1]);
	} else {
		let a = args.shift();
		return EuclideanAlgorithm(a, EuclideanAlgorithmN(...args));
	}
}

function EuclideanAlgorithmN(...args) {
	return args.reduce((prev, cur) => EuclideanAlgorithm(prev, cur));
}

function ExtendEuclideanAlgorithm(a, b) {
	a < b && ([a, b] = [b, a]);
	let r = a % b, q = Math.floor(a / b);
	if (r === 0) {
		return [q, r];
	} else {
		let [q1, r1] = ExtendEuclideanAlgorithm(b, r);

	}
}


function EuclideanAlgorithm(a, b) {
	let r = 1;
	
	while (b !== 0) {
		a < b && ([a, b] = [b, a]);
		r = a % b;
		a = b;
		b = r;
	}
	return a;
}

console.log(EuclideanAlgorithm(8, 4));
console.log(EuclideanAlgorithm(18, 24));
// console.log(EuclideanAlgorithmN(8, 4, 2));
// console.log(EuclideanAlgorithmN(24, 18, 12));