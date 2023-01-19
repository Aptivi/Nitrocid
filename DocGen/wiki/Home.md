
> [!IMPORTANT]
> This documentation is no longer updated and will be removed at the end of the development cycle of 0.1.0. Consult the new documentation hosted on [GitBook](https://aptivi.gitbook.io/kernel-simulator-manual/).

# Kernel Simulator

Kernel Simulator is an application which simulates our future-planned kernel as we've imagined it. It has all the usual applications, including filesystem manipulation, kernel mods, basic scripting, and many more awesome features.

## How to boot with GRILO?

Place all Kernel Simulator files to `%localappdata%/GRILO/Bootables/ks/` in Windows or `~/.config/grilo/Bootables/ks` in Linux.

## List of supported versions

Here are all the listed versions of Kernel Simulator which is either supported or out of support. You can also see the release date and the expected end of life date.

> [!IMPORTANT]
> If your kernel has the status of Reached EOL, you must upgrade to the supported version. Otherwise, you can get support from us as long as the version you're running is supported.

### Compatibility warnings

Each revision of the kernel API has several incompatibilities that break the existing mods. The examples are:

- KS 0.0.16.x series or later is not backwards-compatible with the 0.0.15.x series or below. To remedy this issue for kernel configurations, use the KSConverter application supplied within Kernel Simulator.
- Second-generation versions of KS are not backwards-compatible with the first-generation versions. Upgrade your mods to support the latest API changes, making changes if necessary.
- Third-generation versions of KS are not backwards-compatible with the second-generation versions. Upgrade your mods to support the latest API changes, making changes if necessary.

### First-generation (API v1.0)

> [!IMPORTANT]
> This revision of the first-generation version is no longer supported as of 5/20/2021.

| Version    | Release date | EOL date    | EOL status  |
|:----------:|:------------:|:-----------:|:-----------:|
| v0.0.1.0   |  2/22/2018   |  2/22/2019  | Reached EOL |
| v0.0.1.1   |  3/16/2018   |  2/22/2019  | Reached EOL |
| v0.0.2.0   |  3/31/2018   |  3/31/2019  | Reached EOL |
| v0.0.2.1   |  4/5/2018    |  3/31/2019  | Reached EOL |
| v0.0.2.2   |  4/9/2018    |  3/31/2019  | Reached EOL |
| v0.0.2.3   |  4/11/2018   |  3/31/2019  | Reached EOL |
| v0.0.3.0   |  4/30/2018   |  4/30/2019  | Reached EOL |
| v0.0.3.1   |  5/2/2018    |  4/30/2019  | Reached EOL |
| v0.0.4.0   |  5/20/2018   |  5/20/2021  | Reached EOL |
| v0.0.4.1   |  5/22/2018   |  5/20/2021  | Reached EOL |
| v0.0.4.5   |  7/15/2018   |  5/20/2021  | Reached EOL |
| v0.0.4.6   |  7/16/2018   |  5/20/2021  | Reached EOL |
| v0.0.4.7   |  7/17/2018   |  5/20/2021  | Reached EOL |
| v0.0.4.8   |  4/30/2021   |  5/20/2021  | Reached EOL |
| v0.0.4.9   |  7/21/2018   |  9/4/2018   | Reached EOL |
| v0.0.4.10  |  8/1/2018    |  9/4/2018   | Reached EOL |
| v0.0.4.11  |  8/3/2018    |  9/4/2018   | Reached EOL |
| v0.0.4.12  |  8/16/2018   |  9/4/2018   | Reached EOL |
| v0.0.5.0   |  9/4/2018    |  9/4/2019   | Reached EOL |
| v0.0.5.1   |  9/6/2018    |  9/4/2019   | Reached EOL |
| v0.0.5.2   |  9/9/2018    |  9/4/2019   | Reached EOL |
| v0.0.5.5   |  9/22/2018   |  9/4/2019   | Reached EOL |
| v0.0.5.6   |  10/12/2018  |  9/4/2019   | Reached EOL |
| v0.0.5.7   |  10/13/2018  |  9/4/2019   | Reached EOL |
| v0.0.5.8   |  11/1/2018   |  9/4/2019   | Reached EOL |
| v0.0.5.9   |  12/24/2018  |  6/19/2019  | Reached EOL |
| v0.0.5.10  |  2/16/2019   |  6/19/2019  | Reached EOL |
| v0.0.5.11  |  2/18/2019   |  6/19/2019  | Reached EOL |
| v0.0.5.12  |  2/22/2019   |  6/19/2019  | Reached EOL |
| v0.0.5.13  |  4/14/2019   |  6/19/2019  | Reached EOL |
| v0.0.5.14  |  6/13/2019   |  6/19/2019  | Reached EOL |
| v0.0.6.0   |  6/19/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.1   |  6/21/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.2   |  6/24/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.2a  |  6/24/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.2b  |  6/25/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.3   |  6/26/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.4   |  6/28/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.4a  |  7/6/2019    |  6/19/2020  | Reached EOL |
| v0.0.6.4b  |  7/7/2019    |  6/19/2020  | Reached EOL |
| v0.0.6.5   |  7/25/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.6   |  7/26/2019   |  6/19/2020  | Reached EOL |
| v0.0.6.9   |  7/27/2019   |  8/30/2019  | Reached EOL |
| v0.0.6.10  |  8/8/2019    |  8/30/2019  | Reached EOL |
| v0.0.6.11  |  8/10/2019   |  8/30/2019  | Reached EOL |
| v0.0.6.12  |  8/11/2019   |  8/30/2019  | Reached EOL |
| v0.0.6.13  |  8/13/2019   |  8/30/2019  | Reached EOL |
| v0.0.6.13N |  8/16/2019   |  8/30/2019  | Reached EOL |
| v0.0.6.14  |  8/25/2019   |  8/30/2019  | Reached EOL |
| v0.0.7.0   |  8/30/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.1   |  8/31/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.11  |  9/3/2019    |  8/30/2020  | Reached EOL |
| v0.0.7.12  |  9/5/2019    |  8/30/2020  | Reached EOL |
| v0.0.7.13  |  9/15/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.14  |  9/21/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.2   |  9/23/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.21  |  9/29/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.3   |  10/4/2019   |  8/30/2020  | Reached EOL |
| v0.0.7.4   |  10/18/2019  |  8/30/2020  | Reached EOL |
| v0.0.7.41  |  10/19/2019  |  8/30/2020  | Reached EOL |
| v0.0.7.5   |  10/24/2019  |  8/30/2020  | Reached EOL |
| v0.0.7.6   |  10/29/2019  |  8/30/2020  | Reached EOL |
| v0.0.7.61  |  10/30/2019  |  8/30/2020  | Reached EOL |

### First-generation (API v1.1)

> [!WARNING]
> This revision of the first-generation version is supported until 2/22/2023.

| Version    | Release date | EOL date    | EOL status  |
|:----------:|:------------:|:-----------:|:-----------:|
| v0.0.8.0   |  2/22/2020   |  2/22/2023  | Supported   |
| v0.0.8.1   |  3/16/2020   |  2/22/2023  | Supported   |
| v0.0.8.5   |  3/22/2020   |  2/22/2023  | Supported   |
| v0.0.8.6   |  7/14/2021   |  2/22/2023  | Supported   |
| v0.0.8.7   |  8/22/2021   |  2/22/2023  | Supported   |
| v0.0.8.8   |  8/24/2021   |  2/22/2023  | Supported   |
| v0.0.8.9   |  2/8/2022    |  2/22/2023  | Supported   |
| v0.0.8.10  |  3/3/2022    |  2/22/2023  | Supported   |
| v0.0.8.11  |  4/5/2022    |  2/22/2023  | Supported   |
| v0.0.8.12  |  5/11/2022   |  2/22/2023  | Supported   |
| v0.0.8.13  |  6/10/2022   |  2/22/2023  | Supported   |
| v0.0.8.14  |  7/9/2022    |  2/22/2023  | Supported   |
| v0.0.9.0   |  4/23/2020   |  4/23/2021  | Reached EOL |
| v0.0.9.1   |  5/6/2020    |  4/23/2021  | Reached EOL |
| v0.0.10.0  |  5/19/2020   |  5/19/2021  | Reached EOL |
| v0.0.10.1  |  7/8/2020    |  5/19/2021  | Reached EOL |
| v0.0.10.2  |  4/14/2021   |  5/19/2021  | Reached EOL |
| v0.0.11.0  |  7/25/2020   |  7/25/2021  | Reached EOL |
| v0.0.11.1  |  8/6/2020    |  7/25/2021  | Reached EOL |
| v0.0.11.2  |  7/4/2021    |  7/25/2021  | Reached EOL |

### First-generation (API v1.2)

> [!WARNING]
> This revision of the first-generation version is supported until 11/5/2023.

| Version    | Release date | EOL date    | EOL status  |
|:----------:|:------------:|:-----------:|:-----------:|
| v0.0.12.0  |  11/6/2020   |  11/6/2023  | Supported   |
| v0.0.12.1  |  11/22/2020  |  11/6/2023  | Supported   |
| v0.0.12.2  |  11/29/2020  |  11/6/2023  | Supported   |
| v0.0.12.3  |  12/8/2020   |  11/6/2023  | Supported   |
| v0.0.12.4  |  8/19/2021   |  11/6/2023  | Supported   |
| v0.0.12.5  |  2/5/2022    |  11/6/2023  | Supported   |
| v0.0.12.6  |  3/3/2022    |  11/6/2023  | Supported   |
| v0.0.12.7  |  4/5/2022    |  11/6/2023  | Supported   |
| v0.0.12.8  |  5/11/2022   |  11/6/2023  | Supported   |
| v0.0.12.9  |  6/10/2022   |  11/6/2023  | Supported   |
| v0.0.12.10 |  7/9/2022    |  11/6/2023  | Supported   |
| v0.0.12.11 |  8/5/2022    |  11/6/2023  | Supported   |
| v0.0.13.0  |  12/20/2020  |  12/20/2021 | Reached EOL |
| v0.0.13.1  |  12/24/2020  |  12/20/2021 | Reached EOL |
| v0.0.13.2  |  8/19/2021   |  12/20/2021 | Reached EOL |
| v0.0.14.0  |  1/21/2021   |  1/21/2022  | Reached EOL |
| v0.0.14.1  |  1/29/2021   |  1/21/2022  | Reached EOL |
| v0.0.14.2  |  2/1/2021    |  1/21/2022  | Reached EOL |
| v0.0.14.3  |  8/29/2021   |  1/21/2022  | Reached EOL |
| v0.0.15.0  |  2/22/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.1  |  2/23/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.2  |  3/4/2021    |  2/20/2022  | Reached EOL |
| v0.0.15.3  |  3/9/2021    |  2/20/2022  | Reached EOL |
| v0.0.15.4  |  3/12/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.5  |  3/12/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.6  |  3/17/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.7  |  3/21/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.8  |  4/14/2021   |  2/20/2022  | Reached EOL |
| v0.0.15.9  |  8/29/2021   |  2/20/2022  | Reached EOL |

### First-generation (API v1.3)

> [!WARNING]
> This revision of the first-generation version is supported until 6/12/2024.

| Version    | Release date | EOL date    | EOL status  |
|:----------:|:------------:|:-----------:|:-----------:|
| v0.0.16.0  |  6/12/2021   |  6/12/2024  | Supported   |
| v0.0.16.2  |  6/12/2021   |  6/12/2024  | Supported   |
| v0.0.16.3  |  6/14/2021   |  6/12/2024  | Supported   |
| v0.0.16.4  |  6/18/2021   |  6/12/2024  | Supported   |
| v0.0.16.5  |  7/25/2021   |  6/12/2024  | Supported   |
| v0.0.16.6  |  8/2/2021    |  6/12/2024  | Supported   |
| v0.0.16.7  |  8/19/2021   |  6/12/2024  | Supported   |
| v0.0.16.8  |  2/5/2022    |  6/12/2024  | Supported   |
| v0.0.16.9  |  2/28/2022   |  6/12/2024  | Supported   |
| v0.0.16.10 |  3/3/2022    |  6/12/2024  | Supported   |
| v0.0.16.11 |  3/4/2022    |  6/12/2024  | Supported   |
| v0.0.16.12 |  4/5/2022    |  6/12/2024  | Supported   |
| v0.0.16.13 |  5/11/2022   |  6/12/2024  | Supported   |
| v0.0.16.14 |  6/10/2022   |  6/12/2024  | Supported   |
| v0.0.16.15 |  7/9/2022    |  6/12/2024  | Supported   |
| v0.0.16.16 |  8/5/2022    |  6/12/2024  | Supported   |
| v0.0.17.0  |  7/4/2021    |  7/4/2022   | Reached EOL |
| v0.0.17.1  |  7/17/2021   |  7/4/2022   | Reached EOL |
| v0.0.17.2  |  7/25/2021   |  7/4/2022   | Reached EOL |
| v0.0.17.3  |  8/2/2021    |  7/4/2022   | Reached EOL |
| v0.0.17.4  |  8/25/2021   |  7/4/2022   | Reached EOL |
| v0.0.17.5  |  2/5/2022    |  7/4/2022   | Reached EOL |
| v0.0.17.6  |  4/5/2022    |  7/4/2022   | Reached EOL |
| v0.0.17.7  |  5/11/2022   |  7/4/2022   | Reached EOL |
| v0.0.17.8  |  6/10/2022   |  7/4/2022   | Reached EOL |
| v0.0.18.0  |  8/2/2021    |  8/2/2022   | Reached EOL |
| v0.0.18.1  |  8/9/2021    |  8/2/2022   | Reached EOL |
| v0.0.18.2  |  8/24/2021   |  8/2/2022   | Reached EOL |
| v0.0.18.3  |  2/5/2022    |  8/2/2022   | Reached EOL |
| v0.0.18.4  |  4/5/2022    |  8/2/2022   | Reached EOL |
| v0.0.18.5  |  5/11/2022   |  8/2/2022   | Reached EOL |
| v0.0.18.6  |  6/10/2022   |  8/2/2022   | Reached EOL |
| v0.0.18.7  |  7/9/2022    |  8/2/2022   | Reached EOL |
| v0.0.19.0  |  8/24/2021   |  8/24/2022  | Reached EOL |
| v0.0.19.1  |  8/26/2021   |  8/24/2022  | Reached EOL |
| v0.0.19.2  |  2/5/2022    |  8/24/2022  | Reached EOL |
| v0.0.19.3  |  3/3/2022    |  8/24/2022  | Reached EOL |
| v0.0.19.4  |  4/5/2022    |  8/24/2022  | Reached EOL |
| v0.0.19.5  |  5/11/2022   |  8/24/2022  | Reached EOL |
| v0.0.19.6  |  6/10/2022   |  8/24/2022  | Reached EOL |
| v0.0.19.7  |  7/9/2022    |  8/24/2022  | Reached EOL |
| v0.0.19.8  |  8/5/2022    |  8/24/2022  | Reached EOL |

### Second-generation (API v2.0)

> [!NOTE]
> This revision of the second-generation version is supported until 2/22/2025, and the security support will end at 2/22/2027.

| Version    | Release date | EOL date    | Security EOL date | EOL status  |
|:----------:|:------------:|:-----------:|:-----------------:|:-----------:|
| v0.0.20.0  |  2/22/2022   |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.1  |  3/2/2022    |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.2  |  3/13/2022   |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.3  |  3/19/2022   |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.4  |  4/5/2022    |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.5  |  4/14/2022   |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.6  |  5/5/2022    |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.7  |  6/10/2022   |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.8  |  7/8/2022    |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.20.9  |  8/5/2022    |  2/22/2025  |  2/22/2027        | Supported   |
| v0.0.21.0  |  4/28/2022   |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.1  |  5/1/2022    |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.2  |  5/4/2022    |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.3  |  5/8/2022    |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.4  |  5/10/2022   |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.5  |  5/16/2022   |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.6  |  6/10/2022   |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.7  |  7/8/2022    |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.21.8  |  8/5/2022    |  4/28/2023  |  4/28/2023        | Supported   |
| v0.0.22.0  |  6/12/2022   |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.22.1  |  6/12/2022   |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.22.2  |  6/13/2022   |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.22.3  |  6/15/2022   |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.22.4  |  6/17/2022   |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.22.5  |  7/8/2022    |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.22.6  |  8/5/2022    |  6/12/2023  |  6/12/2023        | Supported   |
| v0.0.23.0  |  7/12/2022   |  7/12/2023  |  7/12/2023        | Supported   |
| v0.0.23.1  |  7/12/2022   |  7/12/2023  |  7/12/2023        | Supported   |
| v0.0.23.2  |  7/13/2022   |  7/12/2023  |  7/12/2023        | Supported   |
| v0.0.23.2  |  8/5/2022    |  7/12/2023  |  7/12/2023        | Supported   |

### Second-generation (API v2.1)

> [!NOTE]
> This revision of the second-generation version is supported until 8/2/2025, and the security support will end at 8/2/2027.

| Version    | Release date | EOL date    | Security EOL date | EOL status  |
|:----------:|:------------:|:-----------:|:-----------------:|:-----------:|
| v0.0.24.0  |  8/2/2022    |  8/2/2025   |  8/2/2027         | Supported   |
| v0.0.24.1  |  8/3/2022    |  8/2/2025   |  8/2/2027         | Supported   |
| v0.0.24.2  |  8/5/2022    |  8/2/2025   |  8/2/2027         | Supported   |
| v0.0.24.3  |  8/9/2022    |  8/2/2025   |  8/2/2027         | Supported   |
| v0.0.24.4  |  8/10/2022   |  8/2/2025   |  8/2/2027         | Supported   |

### Third generation (API v3.0)

> [!NOTE]
> This revision of the third-generation version is still under active development.

| Version    | Release date | EOL date    | Security EOL date | EOL status  |
|:----------:|:------------:|:-----------:|:-----------------:|:-----------:|
| v0.1.0     |  ???         |  ???        |  ???              | ???         |
