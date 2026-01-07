//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace Nitrocid.ScreensaverPacks.Screensavers.Utilities
{
    internal static class HackingCodeList
    {
        // Taken from https://www.cprogramming.com/snippets/list.php?page=$page&count=$count&orderBy=tip
        internal static string[] codeSnippets =
        [
            """
            void stack_push(Sentinel *sentinel, node_data d)
            {
                Node *node_new = malloc(sizeof(Node));
                if(!node_new) {puts("Out of memory"); exit(EXIT_FAILURE);}

                if(!(sentinel -> last))
                    sentinel -> last = node_new;

                node_new -> data = d;
                node_new -> next = sentinel -> head;
                sentinel -> head = node_new;

                ++(sentinel -> count);
                sentinel -> sum += d;
                if(d > sentinel -> max)
                    sentinel -> max = d;
                if(d < sentinel -> min)
                    sentinel -> min = d;
            }
            """,

            """
            void mergesort(int a[], int low, int high) {
                int i = 0;
                int length = high - low + 1;
                int pivot  = 0;
                int merge1 = 0;
                int merge2 = 0;
                int working[length];

                if(low == high)
                    return;

                pivot  = (low + high) / 2;

                mergesort(a, low, pivot);
                mergesort(a, pivot + 1, high);

                for(i = 0; i < length; i++)
                working[i] = a[low + i];

                merge1 = 0;
                merge2 = pivot - low + 1;

                for(i = 0; i < length; i++) {
                if(merge2 <= high - low)
                    if(merge1 <= pivot - low)
                    if(working[merge1] > working[merge2])
                        a[i + low] = working[merge2++];
                    else
                        a[i + low] = working[merge1++];
                    else
                    a[i + low] = working[merge2++];
                else
                    a[i + low] = working[merge1++];
                }
            }
            """,

            """
            void heapsort(int *arr, int n)
            {
                int start, end;

                // heapify the array
                for(start = (n - 2) / 2; start >= 0; --start) // for every root
                    siftDown(arr, start, n); // sift through it

                // sort the array
                for(end = n - 1; end; --end) // for every element of the heap
                {
                    swapi(&arr[end], &arr[0]); // swap it with the top root
                    siftDown(arr, 0, end); // rebuild the heap
                }
            }

            void siftDown(int *arr, int start, int end)
            {
                int root, child;

                root = start; // pick the root index
                while(2 * root + 1 < end) // while the root has a child
                {
                    child = 2 * root + 1; // pick its index
                    if((child + 1 < end) && (arr[child] < arr[child+1]))
                        ++child; // if the other child is bigger, pick it instead
                    if(arr[root] < arr[child]) // if root is smaller than the child
                    {
                        swapi(&arr[child], &arr[root]); // swap them
                        root = child; // go down the heap
                    }
                    else // if the child is smaller than the root
                        return; // that root is in the right spot
                }
            }

            void swapi(int *x, int *y)
            {
                int z;

                z = *x;
                *x = *y;
                *y = z;
            }
            """,

            // Taken from https://rosettacode.org/wiki/Category:C
            """
            bool is_prime(int n) {
                if (n < 2) return false;
                if (n % 2 == 0 && n != 2) return false;
                for (int i = 3; i <= sqrt(n); i += 2) {
                    if (n % i == 0) return false;
                }
                return true;
            }

            int phi(int n) {
                int result = n;
                for (int p = 2; p * p <= n; p++) {
                    if (n % p == 0) {
                        while (n % p == 0) n /= p;
                        result -= result / p;
                    }
                }
                if (n > 1) result -= result / n;
                return result;
            }

            bool is_powerful(int n) {
                int m = n;
                for (int p = 2; p * p <= n; p++) {
                    if (m % p == 0) {
                        int exp = 0;
                        while (m % p == 0) {
                            m /= p;
                            exp++;
                        }
                        if (exp < 2) return false; 
                    }
                }
                if (m > 1) return false; 
                return true;
            }

            bool is_perfect_power(int n) {
                for (int k = 2; k <= log2(n); k++) {
                    double root = pow(n, 1.0 / k);
                    int r = round(root);
                    if (pow(r, k) == n) return true;
                }
                return false;
            }
            """,

            """
            void append(node *root, node *elem) {
                if (root == NULL) {
                    fprintf(stderr, "Cannot append to uninitialized node.");
                    exit(1);
                }
                if (elem == NULL) {
                    return;
                }

                if (root->tag == NODE_SEQ || root->tag == NODE_TREE) {
                    if (root->data.root == NULL) {
                        root->data.root = elem;
                    } else {
                        node *it = root->data.root;
                        while (it->next != NULL) {
                            it = it->next;
                        }
                        it->next = elem;
                    }
                } else {
                    fprintf(stderr, "Cannot append to node with tag: %d\n", root->tag);
                    exit(1);
                }
            }

            size_t count(node *n) {
                if (n == NULL) {
                    return 0;
                }

                if (n->tag == NODE_LEAF) {
                    return 1;
                }
                if (n->tag == NODE_TREE) {
                    size_t sum = 0;
                    node *it = n->data.root;
                    while (it != NULL) {
                        sum += count(it);
                        it = it->next;
                    }
                    return sum;
                }
                if (n->tag == NODE_SEQ) {
                    size_t prod = 1;
                    node *it = n->data.root;
                    while (it != NULL) {
                        prod *= count(it);
                        it = it->next;
                    }
                    return prod;
                }

                fprintf(stderr, "Cannot count node with tag: %d\n", n->tag);
                exit(1);
            }

            void expand(node *n, size_t pos) {
                if (n == NULL) {
                    return;
                }

                if (n->tag == NODE_LEAF) {
                    printf(n->data.str);
                } else if (n->tag == NODE_TREE) {
                    node *it = n->data.root;
                    while (true) {
                        size_t cnt = count(it);
                        if (pos < cnt) {
                            expand(it, pos);
                            break;
                        }
                        pos -= cnt;
                        it = it->next;
                    }
                } else if (n->tag == NODE_SEQ) {
                    size_t prod = pos;
                    node *it = n->data.root;
                    while (it != NULL) {
                        size_t cnt = count(it);

                        size_t rem = prod % cnt;
                        expand(it, rem);

                        it = it->next;
                    }
                } else {
                    fprintf(stderr, "Cannot expand node with tag: %d\n", n->tag);
                    exit(1);
                }
            }
            """,

            """
            ull binomial(ull m, ull n)
            {
            	ull r = 1, d = m - n;
            	if (d > n) { n = d; d = m - n; }

            	while (m > n) {
            		r *= m--;
            		while (d > 1 && ! (r%d) ) r /= d--;
            	}

            	return r;
            }

            ull catalan1(int n) {
            	return binomial(2 * n, n) / (1 + n);
            }

            ull catalan2(int n) {
            	int i;
            	ull r = !n;

            	for (i = 0; i < n; i++)
            		r += catalan2(i) * catalan2(n - 1 - i);
            	return r;
            }

            ull catalan3(int n)
            {
            	return n ? 2 * (2 * n - 1) * catalan3(n - 1) / (1 + n) : 1;
            }
            """,

            """
            from PIL import Image
            import colorsys
            import math

            if __name__ == "__main__":

                im = Image.new("RGB", (300,300))
                radius = min(im.size)/2.0
                cx, cy = im.size[0]/2, im.size[1]/2
                pix = im.load()

                for x in range(im.width):
                    for y in range(im.height):
                        rx = x - cx
                        ry = y - cy
                        s = (rx ** 2.0 + ry ** 2.0) ** 0.5 / radius
                        if s <= 1.0:
                            h = ((math.atan2(ry, rx) / math.pi) + 1.0) / 2.0
                            rgb = colorsys.hsv_to_rgb(h, s, 1.0)
                            pix[x,y] = tuple([int(round(c*255.0)) for c in rgb])

                im.show()
            """,

            """
            use image::error::ImageResult;
            use image::{Rgb, RgbImage};

            fn hsv_to_rgb(h: f64, s: f64, v: f64) -> Rgb<u8> {
                let hp = h / 60.0;
                let c = s * v;
                let x = c * (1.0 - (hp % 2.0 - 1.0).abs());
                let m = v - c;
                let mut r = 0.0;
                let mut g = 0.0;
                let mut b = 0.0;
                if hp <= 1.0 {
                    r = c;
                    g = x;
                } else if hp <= 2.0 {
                    r = x;
                    g = c;
                } else if hp <= 3.0 {
                    g = c;
                    b = x;
                } else if hp <= 4.0 {
                    g = x;
                    b = c;
                } else if hp <= 5.0 {
                    r = x;
                    b = c;
                } else {
                    r = c;
                    b = x;
                }
                r += m;
                g += m;
                b += m;
                Rgb([(r * 255.0) as u8, (g * 255.0) as u8, (b * 255.0) as u8])
            }

            fn write_color_wheel(filename: &str, width: u32, height: u32) -> ImageResult<()> {
                let mut image = RgbImage::new(width, height);
                let margin = 10;
                let diameter = std::cmp::min(width, height) - 2 * margin;
                let xoffset = (width - diameter) / 2;
                let yoffset = (height - diameter) / 2;
                let radius = diameter as f64 / 2.0;
                for x in 0..=diameter {
                    let rx = x as f64 - radius;
                    for y in 0..=diameter {
                        let ry = y as f64 - radius;
                        let r = ry.hypot(rx) / radius;
                        if r > 1.0 {
                            continue;
                        }
                        let a = 180.0 + ry.atan2(-rx).to_degrees();
                        image.put_pixel(x + xoffset, y + yoffset, hsv_to_rgb(a, r, 1.0));
                    }
                }
                image.save(filename)
            }
            """,

            """
            uint32_t cycle(uint32_t n) {
                uint32_t m = n, p = 1;
                while (m >= 10) {
                    p *= 10;
                    m /= 10;
                }
                return m + 10 * (n % p);
            }

            bool is_circular_prime(uint32_t p) {
                if (!is_prime(p))
                    return false;
                uint32_t p2 = cycle(p);
                while (p2 != p) {
                    if (p2 < p || !is_prime(p2))
                        return false;
                    p2 = cycle(p2);
                }
                return true;
            }

            void test_repunit(uint32_t digits) {
                char* str = malloc(digits + 1);
                if (str == 0) {
                    fprintf(stderr, "Out of memory\n");
                    exit(1);
                }
                memset(str, '1', digits);
                str[digits] = 0;
                mpz_t bignum;
                mpz_init_set_str(bignum, str, 10);
                free(str);
                if (mpz_probab_prime_p(bignum, 10))
                    printf("R(%u) is probably prime.\n", digits);
                else
                    printf("R(%u) is not prime.\n", digits);
                mpz_clear(bignum);
            }
            """,

            """
            #include<stdlib.h>
            #include<stdio.h>

            int *
            anynacci (int *seedArray, int howMany)
            {
              int *result = malloc (howMany * sizeof (int));
              int i, j, initialCardinality;

              for (i = 0; seedArray[i] != 0; i++);
              initialCardinality = i;

              for (i = 0; i < initialCardinality; i++)
                result[i] = seedArray[i];

              for (i = initialCardinality; i < howMany; i++)
                {
                  result[i] = 0;
                  for (j = i - initialCardinality; j < i; j++)
                    result[i] += result[j];
                }
              return result;
            }
            """,

            """
            #include <stdint.h>
            #include <stdio.h>

            int64_t isqrt(int64_t x) {
                int64_t q = 1, r = 0;
                while (q <= x) {
                    q <<= 2;
                }
                while (q > 1) {
                    int64_t t;
                    q >>= 2;
                    t = x - r - q;
                    r >>= 1;
                    if (t >= 0) {
                        x = t;
                        r += q;
                    }
                }
                return r;
            }
            """,

            """
            #include <stdio.h>
            #include <stdlib.h>
            #include <math.h>
            #include <gsl/gsl_sf_gamma.h>
            #ifndef M_PI
            #define M_PI 3.14159265358979323846
            #endif

            /* very simple approximation */
            double st_gamma(double x)
            {
              return sqrt(2.0*M_PI/x)*pow(x/M_E, x);
            }

            #define A 12
            double sp_gamma(double z)
            {
              const int a = A;
              static double c_space[A];
              static double *c = NULL;
              int k;
              double accm;

              if ( c == NULL ) {
                double k1_factrl = 1.0; /* (k - 1)!*(-1)^k with 0!==1*/
                c = c_space;
                c[0] = sqrt(2.0*M_PI);
                for(k=1; k < a; k++) {
                  c[k] = exp(a-k) * pow(a-k, k-0.5) / k1_factrl;
            	  k1_factrl *= -k;
                }
              }
              accm = c[0];
              for(k=1; k < a; k++) {
                accm += c[k] / ( z + k );
              }
              accm *= exp(-(z+a)) * pow(z+a, z+0.5); /* Gamma(z+1) */
              return accm/z;
            }
            """,

            """
            void walk(int y, int x)
            {
            	if (x < 0 || y < 0 || x > w || y > h) return;

            	if (!x || !y || x == w || y == h) {
            		++count;
            		if (verbose) show();
            		return;
            	}

            	if (vis[y][x]) return;
            	vis[y][x]++; vis[h - y][w - x]++;

            	if (x && !hor[y][x - 1]) {
            		hor[y][x - 1] = hor[h - y][w - x] = 1;
            		walk(y, x - 1);
            		hor[y][x - 1] = hor[h - y][w - x] = 0;
            	}
            	if (x < w && !hor[y][x]) {
            		hor[y][x] = hor[h - y][w - x - 1] = 1;
            		walk(y, x + 1);
            		hor[y][x] = hor[h - y][w - x - 1] = 0;
            	}

            	if (y && !ver[y - 1][x]) {
            		ver[y - 1][x] = ver[h - y][w - x] = 1;
            		walk(y - 1, x);
            		ver[y - 1][x] = ver[h - y][w - x] = 0;
            	}

            	if (y < h && !ver[y][x]) {
            		ver[y][x] = ver[h - y - 1][w - x] = 1;
            		walk(y + 1, x);
            		ver[y][x] = ver[h - y - 1][w - x] = 0;
            	}

            	vis[y][x]--; vis[h - y][w - x]--;
            }

            void cut(void)
            {
            	if (1 & (h * w)) return;

            	hor = alloc2(w + 1, h + 1);
            	ver = alloc2(w + 1, h + 1);
            	vis = alloc2(w + 1, h + 1);

            	if (h & 1) {
            		ver[h/2][w/2] = 1;
            		walk(h / 2, w / 2);
            	} else if (w & 1) {
            		hor[h/2][w/2] = 1;
            		walk(h / 2, w / 2);
            	} else {
            		vis[h/2][w/2] = 1;

            		hor[h/2][w/2-1] = hor[h/2][w/2] = 1;
            		walk(h / 2, w / 2 - 1);
            		hor[h/2][w/2-1] = hor[h/2][w/2] = 0;

            		ver[h/2 - 1][w/2] = ver[h/2][w/2] = 1;
            		walk(h / 2 - 1, w/2);
            	}
            }

            void cwalk(int y, int x, int d)
            {
            	if (!y || y == h || !x || x == w) {
            		++count;
            		return;
            	}
            	vis[y][x] = vis[h-y][w-x] = 1;

            	if (x && !vis[y][x-1])
            		cwalk(y, x - 1, d|1);
            	if ((d&1) && x < w && !vis[y][x+1])
            		cwalk(y, x + 1, d|1);
            	if (y && !vis[y-1][x])
            		cwalk(y - 1, x, d|2);
            	if ((d&2) && y < h && !vis[y + 1][x])
            		cwalk(y + 1, x, d|2);

            	vis[y][x] = vis[h-y][w-x] = 0;
            }

            void count_only(void)
            {
            	int t;
            	long res;
            	if (h * w & 1) return;
            	if (h & 1) t = h, h = w, w = t;

            	vis = alloc2(w + 1, h + 1);
            	vis[h/2][w/2] = 1;

            	if (w & 1) vis[h/2][w/2 + 1] = 1;
            	if (w > 1) {
            		cwalk(h/2, w/2 - 1, 1);
            		res = 2 * count - 1;
            		count = 0;
            		if (w != h)
            			cwalk(h/2+1, w/2, (w & 1) ? 3 : 2);

            		res += 2 * count - !(w & 1);
            	} else {
            		res = 1;
            	}
            	if (w == h) res = 2 * res + 2;
            	count = res;
            }
            """,

            """
            #include <stdbool.h>
            #include <stdio.h>
            #include <stdlib.h>
            #include <string.h>

            int binomial(int n, int k) {
                int num, denom, i;

                if (n < 0 || k < 0 || n < k) return -1;
                if (n == 0 || k == 0) return 1;

                num = 1;
                for (i = k + 1; i <= n; ++i) {
                    num = num * i;
                }

                denom = 1;
                for (i = 2; i <= n - k; ++i) {
                    denom *= i;
                }

                return num / denom;
            }

            int gcd(int a, int b) {
                int temp;
                while (b != 0) {
                    temp = a % b;
                    a = b;
                    b = temp;
                }
                return a;
            }

            typedef struct tFrac {
                int num, denom;
            } Frac;

            Frac makeFrac(int n, int d) {
                Frac result;
                int g;

                if (d == 0) {
                    result.num = 0;
                    result.denom = 0;
                    return result;
                }

                if (n == 0) {
                    d = 1;
                } else if (d < 0) {
                    n = -n;
                    d = -d;
                }

                g = abs(gcd(n, d));
                if (g > 1) {
                    n = n / g;
                    d = d / g;
                }

                result.num = n;
                result.denom = d;
                return result;
            }

            Frac negateFrac(Frac f) {
                return makeFrac(-f.num, f.denom);
            }

            Frac subFrac(Frac lhs, Frac rhs) {
                return makeFrac(lhs.num * rhs.denom - lhs.denom * rhs.num, rhs.denom * lhs.denom);
            }

            Frac multFrac(Frac lhs, Frac rhs) {
                return makeFrac(lhs.num * rhs.num, lhs.denom * rhs.denom);
            }

            bool equalFrac(Frac lhs, Frac rhs) {
                return (lhs.num == rhs.num) && (lhs.denom == rhs.denom);
            }

            bool lessFrac(Frac lhs, Frac rhs) {
                return (lhs.num * rhs.denom) < (rhs.num * lhs.denom);
            }

            void printFrac(Frac f) {
                char buffer[7];
                int len;

                if (f.denom != 1) {
                    snprintf(buffer, 7, "%d/%d", f.num, f.denom);
                } else {
                    snprintf(buffer, 7, "%d", f.num);
                }

                len = 7 - strlen(buffer);
                while (len-- > 0) {
                    putc(' ', stdout);
                }

                printf(buffer);
            }

            Frac bernoulli(int n) {
                Frac a[16];
                int j, m;

                if (n < 0) {
                    a[0].num = 0;
                    a[0].denom = 0;
                    return a[0];
                }

                for (m = 0; m <= n; ++m) {
                    a[m] = makeFrac(1, m + 1);
                    for (j = m; j >= 1; --j) {
                        a[j - 1] = multFrac(subFrac(a[j - 1], a[j]), makeFrac(j, 1));
                    }
                }

                if (n != 1) {
                    return a[0];
                }

                return negateFrac(a[0]);
            }

            void faulhaber(int p) {
                Frac q, *coeffs;
                int j, sign;

                coeffs = malloc(sizeof(Frac)*(p + 1));

                q = makeFrac(1, p + 1);
                sign = -1;
                for (j = 0; j <= p; ++j) {
                    sign = -1 * sign;
                    coeffs[p - j] = multFrac(multFrac(multFrac(q, makeFrac(sign, 1)), makeFrac(binomial(p + 1, j), 1)), bernoulli(j));
                }

                for (j = 0; j <= p; ++j) {
                    printFrac(coeffs[j]);
                }
                printf("\n");

                free(coeffs);
            }
            """,

            """
            use svg::node::element::path::Data;
            use svg::node::element::Path;

            struct HilbertCurve {
                current_x: f64,
                current_y: f64,
                current_angle: i32,
                line_length: f64,
            }

            impl HilbertCurve {
                fn new(x: f64, y: f64, length: f64, angle: i32) -> HilbertCurve {
                    HilbertCurve {
                        current_x: x,
                        current_y: y,
                        current_angle: angle,
                        line_length: length,
                    }
                }
                fn rewrite(order: usize) -> String {
                    let mut str = String::from("A");
                    for _ in 0..order {
                        let mut tmp = String::new();
                        for ch in str.chars() {
                            match ch {
                                'A' => tmp.push_str("-BF+AFA+FB-"),
                                'B' => tmp.push_str("+AF-BFB-FA+"),
                                _ => tmp.push(ch),
                            }
                        }
                        str = tmp;
                    }
                    str
                }
                fn execute(&mut self, order: usize) -> Path {
                    let mut data = Data::new().move_to((self.current_x, self.current_y));
                    for ch in HilbertCurve::rewrite(order).chars() {
                        match ch {
                            'F' => data = self.draw_line(data),
                            '+' => self.turn(90),
                            '-' => self.turn(-90),
                            _ => {}
                        }
                    }
                    Path::new()
                        .set("fill", "none")
                        .set("stroke", "black")
                        .set("stroke-width", "1")
                        .set("d", data)
                }
                fn draw_line(&mut self, data: Data) -> Data {
                    let theta = (self.current_angle as f64).to_radians();
                    self.current_x += self.line_length * theta.cos();
                    self.current_y -= self.line_length * theta.sin();
                    data.line_to((self.current_x, self.current_y))
                }
                fn turn(&mut self, angle: i32) {
                    self.current_angle = (self.current_angle + angle) % 360;
                }
                fn save(file: &str, size: usize, order: usize) -> std::io::Result<()> {
                    use svg::node::element::Rectangle;
                    let x = 10.0;
                    let y = 10.0;
                    let rect = Rectangle::new()
                        .set("width", "100%")
                        .set("height", "100%")
                        .set("fill", "white");
                    let mut hilbert = HilbertCurve::new(x, y, 10.0, 0);
                    let document = svg::Document::new()
                        .set("width", size)
                        .set("height", size)
                        .add(rect)
                        .add(hilbert.execute(order));
                    svg::save(file, &document)
                }
            }

            fn main() {
                HilbertCurve::save("hilbert_curve.svg", 650, 6).unwrap();
            }
            """,

            """
            import javax.swing.* ;
            import java.awt.* ;

            public class Greybars extends JFrame {
               private int width ;
               private int height ;

               public Greybars( )  {
                  super( "grey bars example!" ) ;
                  width = 640 ;
                  height = 320 ;
                  setSize( width , height ) ;
                  setDefaultCloseOperation( JFrame.EXIT_ON_CLOSE ) ;
                  setVisible( true ) ;
                }

                public void paint ( Graphics g ) {
                  int run = 0 ;
                  double colorcomp = 0.0 ; //component of the color
                  for ( int columncount = 8 ; columncount < 128 ; columncount *= 2 ) {
            	 double colorgap = 255.0 / (columncount - 1) ; //by this gap we change the background color
            	 int columnwidth = width / columncount ;
            	 int columnheight = height / 4 ;
            	 if ( run % 2 == 0 ) //switches color directions with every for loop
            	    colorcomp = 0.0 ;
            	 else {
            	    colorcomp = 255.0 ;
            	    colorgap *= -1.0 ;
            	 }
            	 int ystart = 0 + columnheight * run ;
            	 int xstart = 0 ;
            	 for ( int i = 0 ; i < columncount ; i++ ) {
                        int icolor = (int)Math.round(colorcomp) ; //round to nearer integer
            	    Color nextColor = new Color( icolor , icolor, icolor ) ;
            	    g.setColor( nextColor ) ;
            	    g.fillRect( xstart , ystart , columnwidth , columnheight ) ;
            	    xstart += columnwidth ;
            	    colorcomp += colorgap ;
            	 }
            	 run++ ;
                  }
                }

                public static void main( String[ ] args ) {
                   Greybars gb = new Greybars( ) ;
                }
            }
            """,

            """
            use std::iter::repeat;

            fn sierpinski(order: usize) {
                let mut triangle = vec!["*".to_string()];
                for i in 0..order {
                    let space = repeat(' ').take(2_usize.pow(i as u32)).collect::<String>();

                    // save original state
                    let mut d = triangle.clone();

                    // extend existing lines
                    d.iter_mut().for_each(|r| {
                        let new_row = format!("{}{}{}", space, r, space);
                        *r = new_row;
                    });

                    // add new lines
                    triangle.iter().for_each(|r| {
                        let new_row = format!("{}{}{}", r, " ", r);
                        d.push(new_row);
                    });

                    triangle = d;
                }

                triangle.iter().for_each(|r| println!("{}", r));
            }
            fn main() {
                let order = std::env::args()
                    .nth(1)
                    .unwrap_or_else(|| "4".to_string())
                    .parse::<usize>()
                    .unwrap();

                sierpinski(order);
            }
            """,

            """
            #include <stdio.h>
            #include <stdlib.h>
            #include <string.h>

            typedef struct sCarpet {
                int dim;      // dimension
                char *data;   // character data
                char **rows;  // pointers to data rows
            } *Carpet;

            /* Clones a tile into larger carpet, or blank if center */
            void TileCarpet( Carpet d, int r, int c, Carpet tile )
            {
                int y0 = tile->dim*r;
                int x0 = tile->dim*c;
                int k,m;

                if ((r==1) && (c==1)) {
                    for(k=0; k < tile->dim; k++) {
                       for (m=0; m < tile->dim; m++) {
                           d->rows[y0+k][x0+m] = ' ';
                       }
                    }
                }
                else {
                    for(k=0; k < tile->dim; k++) {
                       for (m=0; m < tile->dim; m++) {
                           d->rows[y0+k][x0+m] = tile->rows[k][m];
                       }
                    }
                }
            }

            /* define a 1x1 starting carpet */
            static char s1[]= "#";
            static char *r1[] = {s1};
            static struct sCarpet single = { 1, s1, r1};

            Carpet Sierpinski( int n )
            {
               Carpet carpet;
               Carpet subCarpet;
               int row,col, rb;
               int spc_rqrd;

               subCarpet = (n > 1) ? Sierpinski(n-1) : &single;

               carpet = malloc(sizeof(struct sCarpet));
               carpet->dim = 3*subCarpet->dim;
               spc_rqrd = (2*subCarpet->dim) * (carpet->dim);
               carpet->data = malloc(spc_rqrd*sizeof(char));
               carpet->rows = malloc( carpet->dim*sizeof(char *));
               for (row=0; row<subCarpet->dim; row++) {
                   carpet->rows[row] = carpet->data + row*carpet->dim;
                   rb = row+subCarpet->dim;
                   carpet->rows[rb] = carpet->data + rb*carpet->dim;
                   rb = row+2*subCarpet->dim;
                   carpet->rows[rb] = carpet->data + row*carpet->dim;
               }

                for (col=0; col < 3; col++) {
                  /* 2 rows of tiles to copy - third group points to same data a first */
                  for (row=0; row < 2; row++)
                     TileCarpet( carpet, row, col, subCarpet );
                }
                if (subCarpet != &single ) {
                   free(subCarpet->rows);
                   free(subCarpet->data);
                   free(subCarpet);
                }

                return carpet;
            }

            void CarpetPrint( FILE *fout, Carpet carp)
            {
                char obuf[730];
                int row;
                for (row=0; row < carp->dim; row++) {
                   strncpy(obuf, carp->rows[row], carp->dim);
                   fprintf(fout, "%s\n", obuf);
                }
                fprintf(fout,"\n");
            }
            """,

            """"
            MULTIPLICATION_TABLE = [
                (0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                (1, 2, 3, 4, 0, 6, 7, 8, 9, 5),
                (2, 3, 4, 0, 1, 7, 8, 9, 5, 6),
                (3, 4, 0, 1, 2, 8, 9, 5, 6, 7),
                (4, 0, 1, 2, 3, 9, 5, 6, 7, 8),
                (5, 9, 8, 7, 6, 0, 4, 3, 2, 1),
                (6, 5, 9, 8, 7, 1, 0, 4, 3, 2),
                (7, 6, 5, 9, 8, 2, 1, 0, 4, 3),
                (8, 7, 6, 5, 9, 3, 2, 1, 0, 4),
                (9, 8, 7, 6, 5, 4, 3, 2, 1, 0),
            ]

            INV = (0, 4, 3, 2, 1, 5, 6, 7, 8, 9)

            PERMUTATION_TABLE = [
                (0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                (1, 5, 7, 6, 2, 8, 3, 0, 9, 4),
                (5, 8, 0, 3, 7, 9, 6, 1, 4, 2),
                (8, 9, 1, 6, 0, 4, 3, 5, 2, 7),
                (9, 4, 5, 3, 1, 2, 6, 8, 7, 0),
                (4, 2, 8, 6, 5, 7, 3, 9, 0, 1),
                (2, 7, 9, 3, 8, 0, 6, 4, 1, 5),
                (7, 0, 4, 6, 9, 1, 3, 2, 5, 8),
            ]

            def verhoeffchecksum(n, validate=True, terse=True, verbose=False):
                """
                Calculate the Verhoeff checksum over `n`.
                Terse mode or with single argument: return True if valid (last digit is a correct check digit).
                If checksum mode, return the expected correct checksum digit.
                If validation mode, return True if last digit checks correctly.
                """
                if verbose:
                    print(f"\n{'Validation' if validate else 'Check digit'}",\
                        f"calculations for {n}:\n\n i  nᵢ  p[i,nᵢ]   c\n------------------")
                # transform number list
                c, dig = 0, list(str(n if validate else 10 * n))
                for i, ni in enumerate(dig[::-1]):
                    p = PERMUTATION_TABLE[i % 8][int(ni)]
                    c = MULTIPLICATION_TABLE[c][p]
                    if verbose:
                        print(f"{i:2}  {ni}      {p}    {c}")

                if verbose and not validate:
                    print(f"\ninv({c}) = {INV[c]}")
                if not terse:
                    print(f"\nThe validation for '{n}' is {'correct' if c == 0 else 'incorrect'}."\
                          if validate else f"\nThe check digit for '{n}' is {INV[c]}.")
                return c == 0 if validate else INV[c]

            if __name__ == '__main__':

                for n, va, t, ve in [
                    (236, False, False, True), (2363, True, False, True), (2369, True, False, True),
                    (12345, False, False, True), (123451, True, False, True), (123459, True, False, True),
                    (123456789012, False, False, False), (1234567890120, True, False, False),
                    (1234567890129, True, False, False)]:
                    verhoeffchecksum(n, va, t, ve)
            """",

            """
            typedef struct bit_array_tag {
                uint32_t size;
                uint32_t* array;
            } bit_array;

            bool bit_array_create(bit_array* b, uint32_t size) {
                uint32_t* array = calloc((size + 31)/32, sizeof(uint32_t));
                if (array == NULL)
                    return false;
                b->size = size;
                b->array = array;
                return true;
            }

            void bit_array_destroy(bit_array* b) {
                free(b->array);
                b->array = NULL;
            }

            void bit_array_set(bit_array* b, uint32_t index, bool value) {
                assert(index < b->size);
                uint32_t* p = &b->array[index >> 5];
                uint32_t bit = 1 << (index & 31);
                if (value)
                    *p |= bit;
                else
                    *p &= ~bit;
            }

            bool bit_array_get(const bit_array* b, uint32_t index) {
                assert(index < b->size);
                uint32_t* p = &b->array[index >> 5];
                uint32_t bit = 1 << (index & 31);
                return (*p & bit) != 0;
            }

            typedef struct sieve_tag {
                uint32_t limit;
                bit_array not_prime;
            } sieve;

            bool sieve_create(sieve* s, uint32_t limit) {
                if (!bit_array_create(&s->not_prime, limit/2))
                    return false;
                for (uint32_t p = 3; p * p <= limit; p += 2) {
                    if (bit_array_get(&s->not_prime, p/2 - 1) == false) {
                        uint32_t inc = 2 * p;
                        for (uint32_t q = p * p; q <= limit; q += inc)
                            bit_array_set(&s->not_prime, q/2 - 1, true);
                    }
                }
                s->limit = limit;
                return true;
            }

            void sieve_destroy(sieve* s) {
                bit_array_destroy(&s->not_prime);
            }

            bool is_prime(const sieve* s, uint32_t n) {
                assert(n <= s->limit);
                if (n == 2)
                    return true;
                if (n < 2 || n % 2 == 0)
                    return false;
                return bit_array_get(&s->not_prime, n/2 - 1) == false;
            }

            // return number of decimal digits
            uint32_t count_digits(uint32_t n) {
                uint32_t digits = 0;
                for (; n > 0; ++digits)
                    n /= 10;
                return digits;
            }

            // return the number with one digit replaced
            uint32_t change_digit(uint32_t n, uint32_t index, uint32_t new_digit) {
                uint32_t p = 1;
                uint32_t changed = 0;
                for (; index > 0; p *= 10, n /= 10, --index)
                    changed += p * (n % 10);
                changed += (10 * (n/10) + new_digit) * p;
                return changed;
            }

            // returns true if n unprimeable
            bool unprimeable(const sieve* s, uint32_t n) {
                if (is_prime(s, n))
                    return false;
                uint32_t d = count_digits(n);
                for (uint32_t i = 0; i < d; ++i) {
                    for (uint32_t j = 0; j <= 9; ++j) {
                        uint32_t m = change_digit(n, i, j);
                        if (m != n && is_prime(s, m))
                            return false;
                    }
                }
                return true;
            }
            """
        ];
    }
}
