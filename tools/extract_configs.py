import sys
import os
import UnityPy

DATA_DIR = r"D:\SteamLibrary\steamapps\common\Magicraft\Magicraft_Data"
OUT_DIR = r"D:\SteamLibrary\steamapps\common\Magicraft\_reverse_engineering\assets_export"

os.makedirs(OUT_DIR, exist_ok=True)

def list_names(env, prefix_filter=None):
    names = []
    for obj in env.objects:
        if obj.type.name == "TextAsset":
            try:
                data = obj.read()
                name = data.m_Name
                if prefix_filter is None or prefix_filter.lower() in name.lower():
                    names.append(name)
            except Exception as e:
                names.append(f"<error reading obj {obj.path_id}: {e}>")
    return names

def dump_text_asset(env, target_name, out_path):
    for obj in env.objects:
        if obj.type.name == "TextAsset":
            try:
                data = obj.read()
            except Exception:
                continue
            if data.m_Name == target_name:
                script = data.m_Script
                if isinstance(script, str):
                    raw = script.encode("utf-8", errors="surrogatepass")
                else:
                    raw = bytes(script)
                with open(out_path, "wb") as f:
                    f.write(raw)
                return True
    return False

if __name__ == "__main__":
    mode = sys.argv[1] if len(sys.argv) > 1 else "list"
    print("Loading resources.assets ...")
    env = UnityPy.load(os.path.join(DATA_DIR, "resources.assets"))
    print("Loaded. Total objects:", len(env.objects))

    if mode == "list":
        names = list_names(env)
        print(f"Total TextAsset count: {len(names)}")
        list_path = os.path.join(OUT_DIR, "textasset_names.txt")
        with open(list_path, "w", encoding="utf-8") as f:
            for n in sorted(names):
                f.write(n + "\n")
        print("Wrote name list to", list_path)
    elif mode == "dump":
        targets = sys.argv[2:]
        for t in targets:
            out_path = os.path.join(OUT_DIR, t.replace("/", "_") + ".json")
            ok = dump_text_asset(env, t, out_path)
            print(t, "->", "OK" if ok else "NOT FOUND", out_path)
